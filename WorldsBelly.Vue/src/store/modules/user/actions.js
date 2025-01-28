import api from '@/api/user'
import Vue from 'vue'

export default {
    async fetch_user({ commit }, username) {
      try {
        const response = await api.fetchUser(username);
        commit('set_user', response)
      } catch (error) {
        throw error?.response?.data?.Message;
      }
    },
    async fetch_or_create_user({ commit }) {
      try {
        const response = await api.fetchGetOrCreateUser();
        sessionStorage.setItem('signedinuser', JSON.stringify(response));
        commit('set_active_user', response);
        return response;
      } catch (error) {
        Vue.prototype.$msal.signOut();
      }
    },
    async fetch_active_user({ commit }) {
      try {
        const msalUserId = Vue.prototype.$msal.getProfile()?.idTokenClaims?.oid;
        const userString = sessionStorage.getItem("signedinuser");
        if(userString) {
          const user = JSON.parse(userString);
          commit('set_active_user', user);
          if(user?.adObjectId == msalUserId) return user;
          else sessionStorage.removeItem('signedinuser');
        }
        const response = await api.fetchActiveUser();
        sessionStorage.setItem('signedinuser', JSON.stringify(response));
        commit('set_active_user', response);
        return response;
      } catch (error) {
        Vue.prototype.$msal.signOut();
      }
    },
    async update_active_username({ commit }, username) {
      try {
        sessionStorage.removeItem('signedinuser');
        await api.updateActiveUsername(username);
        const response = await api.fetchActiveUser();
        sessionStorage.setItem('signedinuser', JSON.stringify(response));
        commit('set_active_user', response);
      } catch (error) {
        return error;
      }
    },
    async update_active_user({ commit }, user) {
      try {
        sessionStorage.removeItem('signedinuser');
        await api.updateActiveUser(user);
        const response = await api.fetchActiveUser();
        sessionStorage.setItem('signedinuser', JSON.stringify(response));
        commit('set_active_user', response);
      } catch (error) {
        throw error?.response?.data?.Message;
      }
    },
    async update_active_msalUser({ commit }, user) {
      try {
        sessionStorage.removeItem('signedinuser');
        await api.updateActiveMsalUser(user);
        Vue.prototype.$msal.signOut();
      } catch (error) {
        throw error?.response?.data?.Message;
      }
    },
    async delete_profile(context, code) {
      try {
        await api.deleteProfile(code);
        sessionStorage.removeItem('signedinuser');
        Vue.prototype.$msal.signOut();
      } catch (error) {
        throw error?.response?.data?.Message;
      }
    },
    async send_delete_profile_verification_code() {
      try {
        return await api.sendDeleteProfileVerificationCode();
      } catch (error) {
        throw error?.response?.data?.Message;
      }
    },
    async validate_username({ commit }, username) {
      try {
        return await api.validateUsername(username);
      } catch (error) {
        commit('app/set_appError', error, { root: true })
      }
    },
    async send_email_verification_code({ commit }, payload) {
      try {
        return await api.sendEmailVerificationCode(payload);
      } catch (error) {
        throw error?.response?.data?.Message;
      }
    },
    async fetch_recipe_like({ commit }, id) {
      try {
        return await api.fetchRecipeLike(id);
      } catch (error) {
        commit('app/set_appError', error, { root: true })
      }
    },
    async fetch_recipes_related_to_user({ commit, getters, state }, collection) {

      state.recipesFilterCollection[collection].page = 1;
      const queryFilter = getters.get_recipes_filter_collection(collection);
      try {
        const response = await api.fetchRecipes(queryFilter)
        commit('set_recipes_collection', {collection: collection, items: response.recipes});
      } catch (error) {
        commit('app/set_appError', error, { root: true })
      }
    },
    async fetch_append_recipes_related_to_user({ commit, getters, state }, collection) {
      state.recipesFilterCollection[collection].page += 1;
      const queryFilter = getters.get_recipes_filter_collection(collection);

      try {
        const response = await api.fetchRecipes(queryFilter);
        commit('append_recipes_collection', {collection: collection, items: response.recipes});
        return response
      } catch (error) {
        commit('app/set_appError', error, { root: true })
      }
    },
};
