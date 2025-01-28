import tagSelectableApi from '@/api/tagSelectable'
import _ from "lodash";

export default {
	async fetch_tagSelectable_choices({ commit }) {
		try {
			const response = await tagSelectableApi.fetchTagSelectableChoices()
			commit('set_tagSelectable_choices', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
}
