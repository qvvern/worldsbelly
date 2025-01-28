export default {
	get_recipe(state) {
		return state.recipe
	},
	get_recipe_steps(state) {
		return state.recipeSteps
	},
	get_recipe_user_rating(state) {
		return state.recipeUserRating
	},
	get_recipes(state) {
		return state.recipes
	},
	get_recipes_filter(state) {
		return state.recipesFilter
	},
	get_recipe_draft(state) {
		return state.recipeDraft
	},
	get_recipe_drafts(state) {
		return state.recipeDrafts
	},
	get_recipe_best_served(state) {
		return state.recipeBestServed
	},
	get_recipe_consumer(state) {
		return state.recipeConsumer
	},
	get_recipe_age_group(state) {
		return state.recipeAgeGroup
	},
	get_recipe_difficulty(state) {
		return state.recipeDifficulty
	},
}
