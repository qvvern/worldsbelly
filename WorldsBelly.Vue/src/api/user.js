import axios from 'axios'
import { domain } from "@/utils/constants/ApiConstants.js";
import _ from "lodash"

function fetchUser(username) {
	return axios
		.get(domain + `/api/Users/user/${username}`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function fetchGetOrCreateUser() {
	return axios
		.post(domain + `/api/Users/SignedInUser`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}
function fetchActiveUser() {
	return axios
		.get(domain + `/api/Users/SignedInUser`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}
function updateActiveUsername(username) {
	return axios
		.put(domain + `/api/Users/SignedInUser/${username}`)
		.then((response) => response.data)
		.catch((error) => {
			throw error
		})
}
function updateActiveMsalUser(user) {
	return axios
		.put(domain + `/api/Users/SignedInMsalUser`, user)
		.then((response) => response.data)
		.catch((error) => {
			throw error
		})
}
function updateActiveUser(user) {
	return axios
		.put(domain + `/api/Users/SignedInUser`, user)
		.then((response) => response.data)
		.catch((error) => {
			throw error
		})
}
function validateUsername(username) {
	return axios
		.get(domain + `/api/Users/username/${username}`)
		.then((response) => response.data)
		.catch((error) => {
			throw error
		})
}
function sendEmailVerificationCode(payload) {
	return axios
		.put(domain + `/api/Users/email/submit`, payload)
		.then((response) => response.data)
		.catch((error) => {
			throw error
		})
}
function sendDeleteProfileVerificationCode() {
	return axios
		.put(domain + `/api/Users/delete/submit`)
		.then((response) => response.data)
		.catch((error) => {
			throw error
		})
}
function deleteProfile(code) {
	return axios
		.delete(domain + `/api/Users/delete/${encodeURIComponent(code)}`)
		.then((response) => response.data)
		.catch((error) => {
			throw error
		})
}

function fetchRecipeLike(id) {
	return axios
		.get(domain + `/api/Recipes/${id}/like`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function fetchRecipes(queryFilter) {
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
		.post(domain + `/api/users/Recipes`, newFilter)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}
export default {
  fetchUser,
  fetchGetOrCreateUser,
  fetchActiveUser,
  updateActiveUsername,
  validateUsername,
  updateActiveMsalUser,
  updateActiveUser,
  sendEmailVerificationCode,
  sendDeleteProfileVerificationCode,
  deleteProfile,
  fetchRecipeLike,
  fetchRecipes
}
