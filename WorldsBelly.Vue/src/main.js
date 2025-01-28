import Vue from 'vue'
import App from './App.vue'

/* MULTI LANG */
import i18n from "./lang";

import VueGeolocation from 'vue-browser-geolocation';
Vue.use(VueGeolocation);
/* END OF MULTI LANG */

import router from './router'
import store from './store/store'
import Vuelidate from 'vuelidate'
import './axios'
import VTooltip from 'v-tooltip'

import msalPlugin from "./utils/plugins/msalPlugin";
Vue.use(msalPlugin)
import './components/baseElements/_loadBase'
import './assets/icons/icofont.min.css'
import 'bootstrap-icons/font/bootstrap-icons.css';
import './styles/index.css'

import vuetify from './utils/plugins/vuetify';
import stringPlugin from "./utils/plugins/stringPlugin";
import momentPlugin from "./utils/plugins/momentPlugin";
import "./utils/filters/maxLength";

import guidPlugin from "./utils/plugins/guidPlugin";
Vue.use(guidPlugin);
import calculatePlugin from "./utils/plugins/calculatePlugin";
Vue.use(calculatePlugin);


import connectionPlugin from "./utils/plugins/connectionPlugin";
Vue.use(connectionPlugin);

// Swiper Caurosel
import VueAwesomeSwiper from 'vue-awesome-swiper'
import 'swiper/css/swiper.css'
Vue.use(VueAwesomeSwiper)

import localeRouterPlugin from "./utils/plugins/localeRouterPlugin";
Vue.use(localeRouterPlugin)

Vue.use(Vuelidate)
Vue.use(stringPlugin);
Vue.use(momentPlugin);
Vue.use(VTooltip)


Vue.config.productionTip = false

import clickOutside from './utils/directives/clickOutside';
Vue.directive('click-outside', clickOutside);


new Vue({
  i18n,
	router,
	store,
  vuetify,
  render: h => h(App),
}).$mount('#app')
