function getBrowserLanguage() {
    return (navigator.languages && navigator.languages.length > 0)
        ? navigator.languages[0]
        : (navigator.language || "en");
}