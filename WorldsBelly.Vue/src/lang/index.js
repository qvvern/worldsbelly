import Vue from 'vue'

const language = Vue.observable({ language: null });
Object.defineProperty(Vue.prototype, '$language', {
  get() { return language.language; },
  set(value) { language.language = value; }
});

Vue.prototype.$languages = [
  { id: 20, code: "en", englishName: "English", nativeName: "English" },
  { id: 63, code: "es", englishName: "Spanish", nativeName: "Español" },
  { id: 25, code: "de", englishName: "German", nativeName: "Deutsch" },
  { id: 65, code: "fil", englishName: "Tagalog", nativeName: "Tagalog" },
  { id: 18, code: "da", englishName: "Danish", nativeName: "Dansk" },
]

import VueI18n from 'vue-i18n';
Vue.use(VueI18n);

const i18n = new VueI18n({
  locale: 'en',
  fallbackLocale: {
    'de-CH': ['fr', 'it'],
    'default': ['en']
  },
  messages: {
    en: require("./translations/en.json"),
    da: require("./translations/en.json"),
    es: require("./translations/en.json"),
    de: require("./translations/en.json"),
    fil: require("./translations/en.json"),
  }
});

export default i18n;
