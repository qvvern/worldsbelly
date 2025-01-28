import customTagApi from '@/api/customTag'
import _ from "lodash";

export default {
  async fetch_custom_tags({ commit }, search) {
      try {
        const filter = {
          startAt: 0,
          amount: 50,
          search: search
        }
          const response = await customTagApi.fetchCustomTags(filter);
          commit("set_customTags", response);
      } catch (error) {
          window.error = error;
      }
  },

  async append_custom_tags({ getters, commit }, search) {
      const filter = {
        startAt: getters.get_custom_tags.length,
        amount: 50,
        search: search
      }
      try {
          const response = await customTagApi.fetchCustomTags(filter);
          commit("add_customTags", response);
      } catch (error) {
          window.error = error;
      }
  },
}
