import api from '@/api/language'

export default {
	async fetch_languages({ commit }) {
		try {
			const response = await api.fetchLanguages()
			commit('set_languages', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
}
