import actions from './actions'
import mutations from './mutations'
import getters from './getters'

const state = {
	customTags: [],
}

export default {
	namespaced: true,
	state,
	getters,
	mutations,
	actions,
}
