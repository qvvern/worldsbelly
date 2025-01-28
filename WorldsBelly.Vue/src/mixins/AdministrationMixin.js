export const administrationMixin = {
    computed: {
        menuItems() {
            return [
              {
                  name: this.$t('profile'),
                  icon: "icofont-user",
                  iconSize: "1em",
                  routeName: "Profile",
                  activeRoutes: ["Profile"]
              },
              {
                  name: this.$t('create'),
                  icon: "icofont-food-basket",
                  iconSize: "1.25em",
                  routeName: "RecipeCreate",
                  activeRoutes: ["RecipeCreate"]
              },
              {
                  name: this.$t('drafts'),
                  icon: "icofont-bbq",
                  iconSize: "1.5em",
                  routeName: "RecipeDrafts",
                  activeRoutes: ["RecipeDrafts"]
              },
              {
                  name: this.$t('published'),
                  icon: "icofont-dining-table",
                  iconSize: "1.8em",
                  routeName: "RecipePublished",
                  activeRoutes: ["RecipePublished"]
              }
          ];
        }
    },
    methods: {
      isActive(routeName) {
          let foundItem = this.menuItems.find(_ => _.routeName == routeName);
          if(!foundItem) return;
          for (let i = 0; i < foundItem.activeRoutes.length; i++) {
            let routeMatches = this.$route.matched.filter(_ => _.name == foundItem.activeRoutes[i]);

            if (routeMatches.length > 0) {
              return true;
            }
          }
          return false ;
      },
  },
};
