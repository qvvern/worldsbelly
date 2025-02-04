import actions from './actions'
import mutations from './mutations'
import getters from './getters'

const state = {
	notifications: [],
}

export default {
	namespaced: true,
	state,
	getters,
	mutations,
	actions,
}
