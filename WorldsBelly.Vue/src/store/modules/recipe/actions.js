import api from '@/api/recipe'
import _ from "lodash";

export default {
	async fetch_recipes({ commit, getters, state }) {
		const { pageSize } = state
		const pagination = { pageSize: pageSize }
		const queryFilter = getters.get_recipes_filter

		try {
			const response = await api.fetchRecipes(pagination, queryFilter)
			commit('set_page', 1)
			commit('set_recipes', response.recipes)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async fetch_append_recipes({ commit, getters, state }) {
		const { pageSize } = state
		const nextPage = state.page + 1
		const pagination = { pageSize: pageSize, page: nextPage }
		const queryFilter = getters.get_recipes_filter

		try {
			const response = await api.fetchRecipes(pagination, queryFilter)
			commit('append_recipes', response.recipes)
			return response
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async fetch_recipe({ commit }, recipeId) {
		try {
			const response = await api.fetchRecipe(recipeId)
			commit('set_recipe', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async fetch_recipe_steps({ commit }, recipeId) {
		try {
			const response = await api.fetchRecipeSteps(recipeId)
			commit('set_recipe_steps', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async fetch_recipe_user_rating({ commit }, recipeId) {
		try {
			const response = await api.fetchRecipeUserRating(recipeId)
      if(!response) {
        const newRating = {
          recipeId: recipeId,
          sweet: null,
          sour: null,
          salty: null,
          bitter: null,
          spices: null,
          flavor: null,
          rating: null,
        }
        commit('set_recipe_user_rating', newRating)
      }
      else {
        commit('set_recipe_user_rating', response)
      }
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async submit_recipe_user_rating({ commit, getters }, rating) {
		try {
			const response = await api.submitRecipeUserRating(rating)
      commit('set_recipe_user_rating', response)
      // var recipes = getters?.get_recipes;
      // var recipe = getters?.get_recipe;
      // if(recipes?.length > 0) {
      //   var recipeIndex = recipes.findIndex((_) => _.recipeId == rating.recipeId);
      //   if(recipeIndex > 0) {
      //     if(rating.rating) recipes[recipeIndex].rating = rating.rating;
      //     commit('set_recipes', recipes)
      //   }
      // }
      // if(recipe && recipe.recipeId == rating.recipeId) {
      //   if(rating.rating) recipe.rating = rating.rating;
      //   commit('set_recipe', recipe)
      // }
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	fetch_recipe_local({ commit, getters }, recipeId) {
		try {
      var recipe = getters.get_recipes.find((_) => _.id == recipeId);
      if(!recipe) recipe = getters.get_recipe_drafts.find((_) => _.id == recipeId)
			commit('set_recipe', recipe);
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async create_recipe_draft({ commit, getters }, recipe) {
		try {
			const response = await api.createRecipe(recipe);
			commit('set_recipe_draft', response)
      var drafts = getters?.get_recipe_drafts;
      if(drafts?.length > 0) {
          drafts.push(response)
          commit('set_recipe_drafts', drafts)
      }
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async fetch_recipe_draft({ commit }, id) {
		try {
			const response = await api.fetchRecipeDraft(id)
			commit('set_recipe_draft', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async fetch_recipe_drafts({ commit }) {
		try {
			const response = await api.fetchRecipeDrafts()
			commit('set_recipe_drafts', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async update_recipe_draft({ commit, getters }, recipe) {
		try {
			const response = await api.updateRecipeDraft(recipe);
			commit('set_recipe_draft', response)
      var drafts = getters?.get_recipe_drafts;
      if(drafts?.length > 0) {
        var recipeIndex = drafts.findIndex((_) => _.id == recipe.id);
        if(recipeIndex > 0) {
          drafts[recipeIndex] = response;
          commit('set_recipe_drafts', drafts)
        }
      }

		} catch (error) {
      // console.log(error.response.data.errors);
			commit('app/set_appError', error, { root: true })
		}
	},
	async publish_recipe_draft({ commit }, recipe) {
		try {
			await api.publishRecipeDraft(recipe);
			const response = await api.fetchRecipeDrafts()
			commit('set_recipe_drafts', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async rate_recipe_draft({ commit }, payload) {
		try {
			await api.rateRecipeDraft(payload.id, payload.rating);
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async delete_recipe_draft({ commit }, id) {
		try {
      await api.deleteRecipeDraft(id);
			const response = await api.fetchRecipeDrafts()
			commit('set_recipe_drafts', response)
      commit(
        "app/add_app_alert",
        { message: "Recipe was deleted.", type: "success" },
        { root: true }
      );
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async fetch_recipe_best_served({ commit, getters }) {
		try {
      if(getters.get_recipe_best_served?.length > 0) return;
			const response = await api.fetchRecipeBestServed()
			commit('set_recipe_best_served', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async fetch_recipe_consumer({ commit, getters }) {
		try {
      if(getters.get_recipe_consumer?.length > 0) return;
			const response = await api.fetchRecipeConsumer()
			commit('set_recipe_consumer', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async fetch_recipe_age_group({ commit, getters }) {
		try {
      if(getters.get_recipe_age_group?.length > 0) return;
			const response = await api.fetchRecipeAgeGroup()
			commit('set_recipe_age_group', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async fetch_recipe_difficulty({ commit, getters }) {
		try {
      if(getters.get_recipe_difficulty?.length > 0) return;
			const response = await api.fetchRecipeDifficulty()
			commit('set_recipe_difficulty', response)
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
	async like_recipe({ commit }, id) {
		try {
      await api.likeRecipe(id);
		} catch (error) {
			commit('app/set_appError', error, { root: true })
		}
	},
}
