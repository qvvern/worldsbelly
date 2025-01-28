<template>
    <baseLoading :value="true" overlay :progressSentences="[`${$t('preparing')}`, `${$t('loading')}`]" />
</template>

<script>
import { mapGetters, mapActions } from "vuex";
export default {
  name: 'Login',
	computed: {
      ...mapGetters("user/", ["get_active_user"]),
        msalUser() {
            return this.$msal.getProfile()?.idTokenClaims;
        },
	},
  methods: {
		...mapActions('measurement/', ['fetch_measurements']),
		...mapActions('country/', ['fetch_countries']),
		...mapActions('nutrient/', ['fetch_nutrients']),
		...mapActions('recipe/', ['fetch_recipe_best_served', 'fetch_recipe_consumer', 'fetch_recipe_age_group', 'fetch_recipe_difficulty']),
    redirectHandler() {
      const newUser =  this.$msal.getProfile()?.idTokenClaims?.newUser;
      if(newUser) {
        if(this.$route.query.redirect && this.$route.query.redirect != "/Login") {
         this.$localeRouter.push({name: 'SetUpProfile', query:{ redirect: this.$route.query?.redirect }});
        }
        else {
         this.$localeRouter.push({name: 'SetUpProfile'});
        }
      }
      else {
        if(this.$route.query.redirect && this.$route.query.redirect != "/Login") {
          this.$localeRouter.push({path: this.$route.query.redirect});
        }
        else {
          this.$localeRouter.push("/");
        }
      }
    }
  },
  mounted() {
    if(!this.$msal.isMsalAuthenticated) {
      this.$msal.signInRedirect();
    }
  },
  watch: {
    '$msal.isAuthenticated': {
      handler() {
        if(this.$msal.isAuthenticated) {
          var fetch_measurements = this.fetch_measurements();
          var fetch_countries = this.fetch_countries();
          var fetch_nutrients = this.fetch_nutrients();
          var fetch_recipe_best_served = this.fetch_recipe_best_served();
          var fetch_recipe_consumer = this.fetch_recipe_consumer();
          var fetch_recipe_age_group = this.fetch_recipe_age_group();
          var fetch_recipe_difficulty = this.fetch_recipe_difficulty();
          Promise.all([
            fetch_measurements,
            fetch_countries,
            fetch_nutrients,
            fetch_recipe_best_served,
            fetch_recipe_consumer,
            fetch_recipe_age_group,
            fetch_recipe_difficulty
            ])
          .finally(() => {
            this.redirectHandler();
          });
        }
      }, immediate: true
    },
  }
}
</script>

<style lang="scss">
</style>
