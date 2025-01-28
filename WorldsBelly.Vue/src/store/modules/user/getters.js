import Vue from 'vue'

export default {
  get_user(state) {
      return state.user;
  },
  get_active_user(state) {
    return state.activeUser;
    // try {
    //   const msalUserId = Vue.prototype.$msal.getProfile()?.idTokenClaims?.oid;
    //   if(msalUserId) {
    //     const userString = sessionStorage.getItem("signedinuser");
    //     console.log('userString', userString);
    //     if(userString) {
    //       const user = JSON.parse(userString);
    //       if(user?.adObjectId == msalUserId) return user;
    //       else sessionStorage.removeItem('signedinuser');
    //     }
    //     else {
    //       Vue.prototype.$msal.signOut();
    //     }
    //   }
    //   return null;
    // } catch (error) {
    //   Vue.prototype.$msal.signOut();
    // }
  },
  get_recipes_filter_collection: state => collection => {
      return state.recipesFilterCollection[collection] ?? null;
  },
  get_recipes_collection: state => collection => {
      return state.recipesCollection[collection] ?? null;
  },
}
