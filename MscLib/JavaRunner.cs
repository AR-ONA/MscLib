using System.Diagnostics;

namespace MscLib {
    public class JavaRunner {
        private Process? _javaProcess;

        public event Action<string>? OnOutputReceived;
        public event Action? OnProcessExited;

        public bool IsRunning => _javaProcess != null && !_javaProcess.HasExited;
        public int? ProcessId => IsRunning ? _javaProcess?.Id : null;

        public void Start(string javaExecutablePath, string workingDirectory, string jarFileName, int memoryAllocationMB, string extraArguments = "") {
            if (IsRunning) {
                OnOutputReceived?.Invoke("Process is already running."); return;
            }

            if (!File.Exists(javaExecutablePath))
                throw new FileNotFoundException("Java executable not found.", javaExecutablePath);

            if (!File.Exists(Path.Combine(workingDirectory, jarFileName)))
                throw new FileNotFoundException("JAR file not found in the working directory.", jarFileName);

            var arguments = $"-Xmx{memoryAllocationMB}M -jar {jarFileName} {extraArguments}";

            var processStartInfo = new ProcessStartInfo {
                FileName = javaExecutablePath,
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            _javaProcess = new Process {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };

            _javaProcess.OutputDataReceived += (sender, args) => OnOutputReceived?.Invoke(args.Data);
            _javaProcess.ErrorDataReceived += (sender, args) => OnOutputReceived?.Invoke(args.Data);
            _javaProcess.Exited += (sender, args) => {
                OnProcessExited?.Invoke();
                _javaProcess?.Dispose();
                _javaProcess = null;
            };

            _javaProcess.Start();

            _javaProcess.BeginOutputReadLine();
            _javaProcess.BeginErrorReadLine();
        }

        public async Task SendCommandAsync(string command) {
            if (!IsRunning || _javaProcess == null) {
                OnOutputReceived?.Invoke("Cannot send command, process is not running.");
                return;
            }
            await _javaProcess.StandardInput.WriteLineAsync(command);
        }

        public async Task StopAsync(int timeoutSeconds = 30) {
            if (!IsRunning || _javaProcess == null) return;

            await SendCommandAsync("stop");
            await _javaProcess.WaitForExitAsync(CancellationToken.None).WaitAsync(TimeSpan.FromSeconds(timeoutSeconds));

            if (IsRunning) {
                _javaProcess.Kill();
                OnOutputReceived?.Invoke("Server did not stop gracefully. Process was killed.");
            }
        }


    }
}
