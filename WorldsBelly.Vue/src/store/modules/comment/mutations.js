export default {
  set_comments(state, comments) {
      state.comments = comments;
  },
  prepend_comment(state, comment) {
      state.comments.unshift(comment);
  },
  set_reply(state, reply) {
      state.reply = reply;
  },
}
