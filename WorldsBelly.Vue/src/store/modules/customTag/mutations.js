export default {
  set_customTags(state, customTags) {
		state.customTags = customTags
	},
  add_customTags(state, customTags) {
      state.customTags.push(...customTags);
  },
}
