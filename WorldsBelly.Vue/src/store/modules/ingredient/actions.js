import ingredientApi from '@/api/ingredient'
import _ from "lodash";

export default {
  async fetch_ingredients({ commit }, search) {
      try {
        const filter = {
          startAt: 0,
          amount: 50,
          search: search
        }
          const response = await ingredientApi.fetchIngredients(filter);
          commit("set_ingredients", response);
      } catch (error) {
          window.error = error;
      }
  },
  async fetch_ingredients_by_id({ commit }, ids) {
      try {
        return await ingredientApi.fetchIngredientsById(ids);
      } catch (error) {
          // window.error = error;
      }
  },

  async append_ingredients({ getters, commit }, search) {
      const filter = {
        startAt: getters.get_ingredients.length,
        amount: 50,
        search: search
      }
      try {
          const response = await ingredientApi.fetchIngredients(filter);
          commit("add_ingredients", response);
      } catch (error) {
          window.error = error;
      }
  },
}
