import Vue from 'vue'

export default {
  set_user(state, user) {
      state.user = user;
  },
  set_active_user(state, activeUser) {
      state.activeUser = activeUser;
  },
  set_recipes_filter_collection(state, payload) {
      Vue.set(state.recipesFilterCollection, payload.collection, payload.filter);
  },
  set_recipes_collection(state, payload) {
    Vue.set(state.recipesCollection, payload.collection, payload.items);
  },
  append_recipes_collection(state, payload) {
    state.recipesCollection[payload.collection] = [...state.recipesCollection[payload.collection], ...payload.items]
  },
}
