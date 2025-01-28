import Vue from "vue";
import Vuex from "vuex";

import app from "./modules/app/index";
import recipe from "./modules/recipe/index";
import ingredient from "./modules/ingredient/index";
import measurement from "./modules/measurement/index";
import tagSelectable from "./modules/tagSelectable/index";
import country from "./modules/country/index";
import language from "./modules/language/index";
import notification from "./modules/notification/index";
// import customTag from "./modules/customTag/index";
import nutrient from "./modules/nutrient/index";
import user from "./modules/user/index";
import comment from "./modules/comment/index";

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    app,
    recipe,
    ingredient,
    measurement,
    nutrient,
    tagSelectable,
    country,
    language,
    // customTag,
    notification,
    user,
    comment
  },
})
