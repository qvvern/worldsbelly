import nutrientApi from '@/api/nutrient'
import _ from "lodash";

export default {
	async fetch_nutrients({ commit, getters }) {
		try {
      if(getters.get_nutrients?.length > 0) return;
			const response = await nutrientApi.fetchNutrients()
			commit('set_nutrients', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
}
