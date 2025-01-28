import actions from './actions'
import mutations from './mutations'
import getters from './getters'

const state = {
  appAlerts: [],
  appError: null,
}

export default {
	namespaced: true,
	state,
	getters,
	mutations,
	actions,
}
