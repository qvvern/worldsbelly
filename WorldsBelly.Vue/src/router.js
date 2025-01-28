import Vue from 'vue';
import Router from 'vue-router';
import store from "@/store/store";

Vue.use(Router);

const Home = () => import('@/pages/Home.vue');
const ComingSoon = () => import('@/pages/ComingSoon.vue');
const Recipes = () => import('@/pages/Recipes.vue');
const Login = () => import('@/pages/Login.vue');
const SetUpProfile = () => import('@/pages/SetUpProfile.vue');
const TermsOfService = () => import('@/pages/about/TermsOfService.vue');
const PageNotFound = () => import('./pages/404.vue');
const Administration = () => import('./pages/administration/Administration.vue');

/* admin recipe */
const RecipeCreate = () => import('./pages/administration/RecipeCreate.vue');
const RecipePublished = () => import('./pages/administration/RecipePublished.vue');
const RecipeDrafts = () => import('./pages/administration/RecipeDrafts.vue');
const RecipeDraftsEdit = () => import('./pages/administration/recipeDrafts/RecipeDraftsEdit.vue');
const RecipeDraftsOverview = () => import('./pages/administration/recipeDrafts/RecipeDraftsOverview.vue');

/* profile */
const Profile = () => import('./pages/Profile.vue');
const ProfileRecipes = () => import('./pages/profile/ProfileRecipes.vue');
const ProfileLiked = () => import('./pages/profile/ProfileLiked.vue');
const ProfileReviews = () => import('./pages/profile/ProfileReviews.vue');
const ProfileSettings = () => import('./pages/profile/ProfileSettings.vue');
const Test = () => import('./pages/test.vue');

/* recipes*/
const Recipe = () => import('./pages/Recipe.vue');
const RecipeDialog = () => import('./pages/RecipeDialog.vue');

const originalPush = Router.prototype.push;
Router.prototype.push = function push(location, onResolve, onReject) {
    if (onResolve || onReject) return originalPush.call(this, location, onResolve, onReject);
    return originalPush.call(this, location).catch(err => {
        if (err.name == "NavigationDuplicated") {
            return err;
        }
        // rethrow error
        return Promise.reject(err);
    });
};
const router = new Router({
    mode: "history",
    routes: [
      {
        path: '/test',
        name: 'Test',
        component: Test,
        meta: {
          title: 'Test'
        },
      },
        {
          path: '/login',
          name: 'Login',
          component: Login,
          meta: {
            title: 'Login'
          },
        },
        {
          path: '/SetUpProfile',
          alias: ['/:lang/SetUpProfile'],
          name: 'SetUpProfile',
          component: SetUpProfile,
          meta: {
            requiresAuth: true,
            title: 'Set up Profile'
          },
        },
        {
          path: '/about/termsofservice',
          alias: ['/:lang/about/termsofservice'],
          name: 'TermsOfService',
          component: TermsOfService,
          meta: {
            title: 'Terms Of Service'
          },
        },
        {
          path: '*',
          name: 'PageNotFound',
          component: PageNotFound,
          meta: {
            title: 'Page not found',
          },
        },
        {
            path: "/",
            name: "ComingSoon",
            component: ComingSoon,
            meta: {
              title: 'Coming Soon',
              isHome: true
            },
        },
        {
            path: "/home",
            alias: ['/:lang/home'],
            name: "Home",
            component: Home,
            meta: {
              title: 'Home',
              recipePageAsDialog: true,
            },
        },
        {
            path: "/recipes",
            alias: ['/:lang/recipes'],
            name: "Recipes",
            component: Recipes,
            meta: {
              title: 'Recipes',
              recipePageAsDialog: true,
            },
        },
        {
          path: '/recipes/:id',
          alias: ['/:lang/recipes/:id'],
          name: 'Recipe',
          meta: {
            title: 'Recipe',
          },
          beforeEnter: (to, from, next) => {
            rewriteComponentForDialogPage(to, from, 'recipePageAsDialog', Recipe, RecipeDialog)
            next()
          },
        },
        {
            path: "/administration",
            alias: ['/:lang/administration'],
            name: "administration",
            redirect: "/administration/create",
            component: Administration,
            meta: {
                title: "Administration",
                requiresAuth: true,
            },
            children: [
                {
                    path: "create",
                    alias: ['/:lang/administration/create'],
                    name: "RecipeCreate",
                    component: RecipeCreate,
                    meta: {
                        title: "Create",
                        requiresAuth: true,
                    },
                },
                {
                    path: "published",
                    alias: ['/:lang/administration/published'],
                    name: "RecipePublished",
                    component: RecipePublished,
                    meta: {
                        title: "Published",
                        requiresAuth: true,
                        recipePageAsDialog: true,
                    },
                },
                {
                    path: "drafts",
                    alias: ['/:lang/administration/drafts'],
                    name: "RecipeDrafts",
                    component: RecipeDrafts,
                    redirect: "/administration/drafts/",
                    meta: {
                        title: "Drafts",
                        requiresAuth: true,
                    },
                    children: [
                      {
                        path: "/",
                        // alias: ['/:lang'],
                        name: "RecipeDraftsOverview",
                        component: RecipeDraftsOverview,
                        meta: {
                            title: "Overview",
                            requiresAuth: true,
                        },
                      },
                      {
                        path: ":id",
                        // alias: ['/:lang/:id'],
                        name: "RecipeDraftsEdit",
                        component: RecipeDraftsEdit,
                        meta: {
                            title: "Edit",
                            requiresAuth: true,
                        },
                      }
                    ]
                },
            ],
        },
        {
            path: "/profile/:username",
            alias: [
              '/profile',
              '/:lang/profile',
              '/:lang/profile/:username',
            ],
            name: "Profile",
            component: Profile,
            meta: {
                title: "Profile",
                requiresAuth: false,
            },
            children: [
                {
                    path: "recipes",
                    alias: [
                      '/profile/:username',
                      '/:lang/profile/:username'
                    ],
                    name: "ProfileRecipes",
                    component: ProfileRecipes,
                    meta: {
                        title: "Recipes",
                        requiresAuth: false,
                    },
                },
                {
                    path: "liked",
                    alias: [
                      '/profile/:username',
                      '/:lang/profile/:username'
                    ],
                    name: "ProfileLiked",
                    component: ProfileLiked,
                    meta: {
                        title: "Liked",
                        requiresAuth: false,
                    },
                },
                {
                    path: "reviews",
                    alias: [
                      '/profile/:username',
                      '/:lang/profile/:username'
                    ],
                    name: "ProfileReviews",
                    component: ProfileReviews,
                    meta: {
                        title: "Reviews",
                        requiresAuth: false,
                    },
                },
                {
                    path: "/profile/:username/settings",
                    alias: [
                      '/:lang/profile/:username/settings'
                    ],
                    name: "ProfileSettings",
                    component: ProfileSettings,
                    meta: {
                        title: "Settings",
                        requiresAuth: true,
                    },
                },
            ],
        },
    ],
});

const rewriteComponentForDialogPage = (to, from, metaKey, component, dialogComponent) => {
	const dialogView = from.matched.some((view) => view.meta && view.meta[metaKey])
	if (!dialogView) {
		to.matched[0].components = {
			default: component,
			modal: false,
		}
	}
	if (dialogView) {
		if (from.matched.length > 1) {
			// copy nested router
			const childrenView = from.matched.slice(1, from.matched.length)
			for (let view of childrenView) {
				to.matched.push(view)
			}
		}

		if (to.matched[0].components) {
			// Rewrite components for `default`
			to.matched[0].components.default = from.matched[0].components.default
			// Rewrite components for `dialog`
			to.matched[0].components.dialog = dialogComponent
      // if home add tag
      if(from.name == "Home") {
        to.meta.isHome = true;
      }
      else {
        to.meta.isHome = false;
      }
		}
	}
}

router.beforeEach(async (to,from,next)=> {
    if(to.name != "Login") await Vue.prototype.$msal.authorize();
    if(to.meta.requiresAuth) {
      if(Vue.prototype.$msal.isAuthenticated) {
        next()
      }
      else {
        next({name: 'Login', query:{ redirect: to.fullPath }})
      }
    } else {
      next()
    }
})

router.afterEach((to, from) => {
    Vue.nextTick(() => {
        if (to.meta.title) {
            document.title = `Worldsbelly - ${to.meta.title}`;
        } else {
            document.title = "Worldsbelly";
        }
    });
});

export default router;
