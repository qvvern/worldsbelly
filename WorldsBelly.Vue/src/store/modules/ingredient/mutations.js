export default {
  set_ingredients(state, ingredients) {
		state.ingredients = ingredients
	},
  add_ingredients(state, ingredients) {
      state.ingredients.push(...ingredients);
  },
}
