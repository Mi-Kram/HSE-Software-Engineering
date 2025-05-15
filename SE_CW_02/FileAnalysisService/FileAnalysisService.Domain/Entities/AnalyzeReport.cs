namespace FileAnalysisService.Domain.Entities
{
    /// <summary>
    /// Отчёт анализа работы.
    /// </summary>
    public class AnalyzeReport
    {
        private int workID;
        private int paragraphs;
        private int words;
        private int numbers;
        private int symbols;

        /// <summary>
        /// Идентификатор работы.
        /// </summary>
        public int WorkID
        {
            get => workID;
            set => workID = value;
        }

        /// <summary>
        /// Количество абзацев в работе.
        /// </summary>
        public int Paragraphs
        {
            get => paragraphs;
            set => paragraphs = value >= 0 ? value : throw new ArgumentException("Количество абзацев должно быть больше или равно нулю.");
        }

        /// <summary>
        /// Количество слов в работе.
        /// </summary>
        public int Words
        {
            get => words;
            set => words = value >= 0 ? value : throw new ArgumentException("Количество слов должно быть больше или равно нулю.");
        }

        /// <summary>
        /// Количество чисел в работе.
        /// </summary>
        public int Numbers
        {
            get => numbers;
            set => numbers = value >= 0 ? value : throw new ArgumentException("Количество чисел должно быть больше или равно нулю.");
        }

        /// <summary>
        /// Количество символов в работе.
        /// </summary>
        public int Symbols
        {
            get => symbols;
            set => symbols = value >= 0 ? value : throw new ArgumentException("Количество символов должно быть больше или равно нулю.");
        }
    }
}
