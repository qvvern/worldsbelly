import axios from 'axios'
import Vue from 'vue'

axios.interceptors.request.use(
	async function (config) {

		config.headers['Accept'] = 'application/json';
		config.headers['Content-Type'] = 'application/json';
		config.headers['Language-Id'] = Vue.prototype.$language.id;
    if(Vue.prototype.$msal.isMsalAuthenticated) {
      let token = await Vue.prototype.$msal.acquireTokenRedirect();
      if(token?.accessToken) {
          config.headers.Authorization = `Bearer ${token.accessToken}`
      }
    }

		// Added to due axios removing the header 'Content-Type' if config.data is undefined
		setConfigContentType(config)

		return config
	},
	function (err) {
		return Promise.reject(err)
	}
)

const setConfigContentType = (config) => {
	if ((config.method == 'put' || config.method == 'post') && config.data == undefined) {
		config.data = {}
	}
}

// axios.interceptors.response.use(
// 	(response) => {
// 		return response
// 	},
// 	(error) => {
// 		return Promise.reject(error)
// 	}
// )

