import actions from './actions'
import mutations from './mutations'
import getters from './getters'

const state = {
	nutrients: [],
}

export default {
	namespaced: true,
	state,
	getters,
	mutations,
	actions,
}
