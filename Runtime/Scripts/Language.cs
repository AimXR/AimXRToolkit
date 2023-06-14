namespace AimXRToolkit {
    /// summary
    /// Language class
    /// summary
    public class Language {
        /// summary
        /// French language
        /// summary
        public static Language FR = new Language("fr");
        /// summary
        /// English language
        /// summary
        public static Language EN = new Language("en");
        /// summary
        /// German language
        /// summary
        public static Language DE = new Language("de");
        /// summary
        /// Spanish language
        /// summary
        public static Language ES = new Language("es");
        /// summary
        /// Italian language
        /// summary
        public static Language IT = new Language("it");

        private string code;
        private Language(string code) {
            this.code = code;
        }

        /// summary
        /// Returns the code of the language
        /// summary
        public string toString() {
            return this.code;
        }

        /// summary
        /// Returns the Language class corresponding to the given code
        /// summary
        public static Language fromString(string c) {
            switch (c) {
                case "fr": return FR;
                case "en": return EN;
                case "de": return DE;
                case "es": return ES;
                case "it": return IT;
                default: return EN;
            }
        }
    }
}