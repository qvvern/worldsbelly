import actions from './actions'
import mutations from './mutations'
import getters from './getters'

const state = {
	page: 1,
	pageSize: 50,
  recipeUserRating: null,
	recipe: null,
  recipeSteps: [],
	recipes: [],
  recipesFilter: {},
  recipeDraft: null,
  recipeDrafts: [],
  recipeBestServed: [],
  recipeConsumer: [],
  recipeAgeGroup: [],
  recipeDifficulty: []
}

export default {
	namespaced: true,
	state,
	getters,
	mutations,
	actions,
}
