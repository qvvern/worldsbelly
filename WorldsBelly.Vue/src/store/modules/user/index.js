import actions from './actions'
import mutations from './mutations'
import getters from './getters'

const state = {
  user: null,
  activeUser: null,
  recipesFilterCollection: {},
  recipesCollection: {}
}

export default {
	namespaced: true,
	state,
	getters,
	mutations,
	actions,
}
