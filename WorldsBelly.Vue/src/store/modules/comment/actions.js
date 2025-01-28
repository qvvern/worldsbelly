import api from '@/api/comment'

export default {
    async fetch_comments({ commit }, recipeId) {
      try {
        const response = await api.fetchComments(recipeId);
        commit('set_comments', response)
      } catch (error) {
        throw 'Sorry, we were not able to fetch comments.';
      }
    },
    async create_comment({ commit }, payload) {
      try {
        const response = await api.createComment(payload);
        commit('prepend_comment', response)
      } catch (error) {
        throw 'Sorry, something went wrong';
      }
    },
    async create_reply({ commit }, payload) {
      try {
        return await api.createComment(payload);
      } catch (error) {
        throw 'Sorry, something went wrong';
      }
    },
    async delete_reply({ commit }, id) {
      try {
        await api.deleteComment(id);
      } catch (error) {
        throw 'Sorry, something went wrong';
      }
    },
};
