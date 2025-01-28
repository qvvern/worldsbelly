export default {
  set_recipe(state, recipe) {
		state.recipe = recipe
	},
  set_recipe_user_rating(state, recipeUserRating) {
		state.recipeUserRating = recipeUserRating
	},
  set_recipes(state, recipes) {
		state.recipes = recipes
	},
  set_recipe_steps(state, recipeSteps) {
		state.recipeSteps = recipeSteps
	},
	set_recipes_filter(state, recipesFilter) {
		state.recipesFilter = recipesFilter
	},
	set_page(state, page) {
		state.page = page
	},
	append_recipes(state, recipes) {
		state.recipes = [...state.recipes, ...recipes]
		state.page += 1
	},
  set_recipe_draft(state, recipeDraft) {
		state.recipeDraft = recipeDraft
	},
  set_recipe_drafts(state, recipeDrafts) {
		state.recipeDrafts = recipeDrafts
	},
  set_recipe_best_served(state, recipeBestServed) {
		state.recipeBestServed = recipeBestServed
	},
  set_recipe_consumer(state, recipeConsumer) {
		state.recipeConsumer = recipeConsumer
	},
  set_recipe_age_group(state, recipeAgeGroup) {
		state.recipeAgeGroup = recipeAgeGroup
	},
  set_recipe_difficulty(state, recipeDifficulty) {
		state.recipeDifficulty = recipeDifficulty
	},
}
