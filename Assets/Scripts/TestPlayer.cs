using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace GameBot {

    public class TestPlayer : MonoBehaviour {

        [Range(1, 10)]
        public int searchDepth;
        public string logFolderPath;

        private string logfilePath;

        private void Awake() {

            string logFileName = $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            logfilePath = Path.Combine(logFolderPath, logFileName);

            try {
                Directory.CreateDirectory(logFolderPath);
            } catch (Exception) {
                throw new ArgumentException($"Couldn't create folders at {logFolderPath}");
            }
        }

        private void Start() {
            // TODO: Doesn't stop when the editor stops. Fix this
            PlayGame();
        }

        private async void PlayGame() {

            Board board = new Board();
            Stopwatch stopwatch = new Stopwatch();

            PlayerType currentPlayer = PlayerType.ORDER;
            GameState gameState = GameState.NOONE;
            while (gameState == GameState.NOONE) {

                stopwatch.Start();
                Move move = await Task.Run(() => Bot.GetBestMove(board, searchDepth, currentPlayer));
                stopwatch.Stop();

                long elapsedTime = stopwatch.ElapsedMilliseconds;
                stopwatch.Reset();

                board.MakeMove(move);

                PrintOutput($"{currentPlayer}: {elapsedTime} ms", true);
                PrintOutput(board.GenerateBoardVisualization() + Environment.NewLine, false);

                gameState = BoardScorer.GetGameState(board);

                switch (currentPlayer) {
                    case PlayerType.CHAOS:
                        currentPlayer = PlayerType.ORDER;
                        break;
                    case PlayerType.ORDER:
                        currentPlayer = PlayerType.CHAOS;
                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected player type encountered: {currentPlayer}");
                }

            }

            PrintOutput($"{gameState} won!", true);

        }

        private void PrintOutput(string message, bool showInEditor) {
            if (showInEditor) {
                UnityEngine.Debug.Log(message);
            }
            File.AppendAllText(logfilePath, message + Environment.NewLine);
        }

    }

}
