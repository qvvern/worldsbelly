<template>
  <v-app>
    <router-view name="dialog" />
    <baseLayout>
      <div slot="header" v-if="!isHome">
        <AppHeader v-if="!isMobile" />
        <AppHeaderMobile v-else />
      </div>
      <template slot="content">
        <baseAppError v-if="get_appError" />
        <baseAppAlert
            v-for="(appAlert, i) in get_appAlerts"
            :key="i"
            :index="i"
            :type="appAlert.type"
            :message="appAlert.message"
            :durationType="appAlert.durationType"
            :delay="appAlert.delay"
        />
        <template v-if="isPageReady">
            <router-view  /> <!-- adding this will retrigger page load when closing page dialog :key="$route.fullPath" -->
        </template>
      </template>
    </baseLayout>
  </v-app>
</template>

<script>
import { mapGetters, mapActions, mapMutations } from "vuex";

export default {
	name: 'App',
	components: {
		AppHeader: () => import('@/components/app/AppHeader.vue'),
		AppHeaderMobile: () => import('@/components/app/AppHeaderMobile.vue'),
	},
	data() {
		return {
      isPageReady: false,
      didPageFail: false,
		}
	},
	computed: {
      ...mapGetters("app/", ["get_appError"]),
      ...mapGetters("app/", ["get_appAlerts"]),
      // ...mapGetters('language/', ['get_languages']),
      isHome() {
        return this.$route.name == 'Home' || this.$route?.meta?.isHome ? true : false;
      },
      isMobile() {
        return window.innerWidth <= 800;
      },
	},
  methods: {
		// ...mapActions('app/', ['fetch_application_configuration']),
		...mapMutations('country/', ['set_countries']),
    ...mapMutations('recipe/', [
      'set_recipe_best_served',
      'set_recipe_consumer',
      'set_recipe_age_group',
      'set_recipe_difficulty'
      ]),
		...mapMutations('measurement/', ['set_measurements']),
		...mapMutations('nutrient/', ['set_nutrients']),
		...mapActions('measurement/', ['fetch_measurements']),
		...mapActions('country/', ['fetch_countries']),
		...mapActions('nutrient/', ['fetch_nutrients']),
		...mapActions('language/', ['fetch_languages']),
		...mapActions('recipe/', ['fetch_recipe_best_served', 'fetch_recipe_consumer', 'fetch_recipe_age_group', 'fetch_recipe_difficulty']),
    setLanguage() {
      // try a: if lang is in route
      const firstPath = location.pathname?.split("/")?.[1] ?? '';
      this.$language = this.$languages.find(_ => _.code == firstPath);
      if(!this.$language) {
        // try b: if lang is in localstorage
        const localStorageLang = localStorage.getItem("language");
        this.$language = this.$languages.find(_ => _.code == localStorageLang);
        if(!this.$language) {
             // try c: language from browser
            var languageFromBrowser = window?.navigator?.userLanguage || window?.navigator?.language;
            this.$language = this.$languages.find(_ => _.code == languageFromBrowser);
            if(!this.$language) {
              // try d: from domain
              const domain = location.host;
              if(domain == 'worldsbelly.com') {
                this.$language = this.$languages.find(_ => _.code == 'en');
              }
              else if(domain == 'verdensmave.dk') {
                this.$language = this.$languages.find(_ => _.code == 'da');
              }
              else {
                // try e: default english
                this.$language = this.$languages.find(_ => _.code == 'en');
              }
            }
        }
      }
      this.$i18n.locale = this.$language.code;
      localStorage.setItem("language", this.$language.code);
    }
  },
  async mounted() {
        this.setLanguage();
        if(window.location.pathname == '/login') {
          this.isPageReady = true;
          return;
        }
          // var fetch_languages = this.fetch_languages();
          var fetch_measurements = this.fetch_measurements();
          var fetch_countries = this.fetch_countries();
          var fetch_nutrients = this.fetch_nutrients();
          var fetch_recipe_best_served = this.fetch_recipe_best_served();
          // var fetch_recipe_consumer = this.fetch_recipe_consumer();
          // var fetch_recipe_age_group = this.fetch_recipe_age_group();
          var fetch_recipe_difficulty = this.fetch_recipe_difficulty();
          Promise.all([
            // fetch_languages,
            fetch_measurements,
            fetch_countries,
            fetch_nutrients,
            fetch_recipe_best_served,
            // fetch_recipe_consumer,
            // fetch_recipe_age_group,
            fetch_recipe_difficulty
            ])
          .finally(() => {
              this.isPageReady = true;
          });
  },
  // async mounted() {
  //       await this.fetch_application_configuration().then((config) => {
  //         this.set_countries(config.countries);
  //         this.set_measurements(config.measurements);
  //         this.set_nutrients(config.nutrients);
  //         this.set_recipe_best_served(config.recipeBestServed);
  //         this.set_recipe_consumer(config.recipeConsumer);
  //         this.set_recipe_age_group(config.recipeAgeGroup);
  //         this.set_recipe_difficulty(config.recipeDifficulty);
  //         this.isPageReady = true;
  //       });
  // }
}
</script>
