import actions from './actions'
import mutations from './mutations'
import getters from './getters'

const state = {
  comments: [],
  reply: null,
}

export default {
	namespaced: true,
	state,
	getters,
	mutations,
	actions,
}
