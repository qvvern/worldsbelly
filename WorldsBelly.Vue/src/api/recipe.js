import axios from 'axios'
import { domain } from "@/utils/constants/ApiConstants.js";
import _ from "lodash"

function fetchRecipe(id) {
	return axios
		.get(domain + `/api/Recipes/`+id)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function fetchRecipeSteps(id) {
	return axios
		.get(domain + `/api/Recipes/steps/`+id)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function fetchRecipeUserRating(id) {
	return axios
		.get(domain + `/api/Recipes/${id}/user/rating`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}
function submitRecipeUserRating(payload) {
	return axios
		.post(domain + `/api/Recipes/user/rating`, payload)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function fetchRecipes(pagination, queryFilter) {
  var newFilter = _.cloneDeep(queryFilter);
  Object.keys(newFilter).forEach(key => {
    if (newFilter[key] == null || newFilter[key] == "" || (_.isObject(newFilter[key]) && _.isEmpty(newFilter[key]))) {
      delete newFilter[key];
    }
  });
  if(newFilter.sortDescending) {
    newFilter.sortAscending = false;
    delete newFilter.sortDescending;
  }
	return axios
		.post(domain + `/api/Recipes`, {...pagination, ...newFilter})
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

// function buildParams(filter) {
// 	var string = Object.keys(filter)
// 		.map((key) => `${key}=${filter[key]}`)
// 		.join('&');
//     if(!string) return "";
//     return `?${string}`
// }

function createRecipe(recipe) {
	return axios
		.post(domain + `/api/RecipeDrafts`, recipe)
		.then((response) => response.data)
		.catch((error) => {
			throw error
		})
}

function fetchRecipeDraft(id) {
	return axios
		.get(domain + `/api/RecipeDrafts/${id}`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}
function fetchRecipeDrafts() {
	return axios
		.get(domain + `/api/RecipeDrafts`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function updateRecipeDraft(recipe) {
	return axios
		.put(domain + `/api/RecipeDrafts/${recipe.id}`, recipe)
		.then((response) => response.data)
		.catch((error) => {
			throw error
		})
}
function rateRecipeDraft(recipeId, rating) {
	return axios
		.put(domain + `/api/RecipeDrafts/rate/${recipeId}`, rating)
		.then((response) => response.data)
		.catch((error) => {
			throw error
		})
}

function publishRecipeDraft(recipe) {
	return axios
		.put(domain + `/api/RecipeDrafts/publish/${recipe.id}`, recipe)
		.then((response) => response.data)
		.catch((error) => {
			throw error
		})
}

function deleteRecipeDraft(id) {
	return axios
		.delete(domain + `/api/RecipeDrafts/${id}`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function fetchRecipeBestServed() {
	return axios
		.get(domain + `/api/Recipes/BestServed`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}
function fetchRecipeConsumer() {
	return axios
		.get(domain + `/api/Recipes/Consumer`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}
function fetchRecipeAgeGroup() {
	return axios
		.get(domain + `/api/Recipes/AgeGroup`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}
function fetchRecipeDifficulty() {
	return axios
		.get(domain + `/api/Recipes/Difficulty`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function likeRecipe(id) {
	return axios
		.post(domain + `/api/Recipes/${id}/like`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

export default {
    submitRecipeUserRating,
    fetchRecipe,
    rateRecipeDraft,
    updateRecipeDraft,
    fetchRecipes,
    createRecipe,
    fetchRecipeDraft,
    fetchRecipeDrafts,
    publishRecipeDraft,
    deleteRecipeDraft,
    fetchRecipeBestServed,
    fetchRecipeConsumer,
    fetchRecipeAgeGroup,
    fetchRecipeDifficulty,
    fetchRecipeUserRating,
    fetchRecipeSteps,
    likeRecipe
}
