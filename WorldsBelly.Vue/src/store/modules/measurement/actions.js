import measurementApi from '@/api/measurement'
import _ from "lodash";

export default {
	async fetch_measurements({ commit, getters }) {
		try {
      if(getters.get_measurements?.length > 0) return;
			const response = await measurementApi.fetchMeasurements()
			commit('set_measurements', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
}
