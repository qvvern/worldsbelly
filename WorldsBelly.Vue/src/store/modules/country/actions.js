import countryApi from '@/api/country'
import _ from "lodash";

export default {
	async fetch_countries({ commit, getters }) {
		try {
			const response = await countryApi.fetchCountries()
			commit('set_countries', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
}
