<template>
    <baseLayout class="home" scrollListener @scroll="handleScroll">
        <div slot="content" class="home__body">
            <AppHeader v-if="!isMobile" style="position: sticky; top: 0; z-index: 100" :style="appStylings" :isHome="!changeHeader" />
            <AppHeaderMobile v-else style="position: sticky; top: 0; z-index: 100" :style="appStylings" :isHome="!changeHeader" />
            <v-toolbar
                style="margin-top: -60px"
                class="home-toolbar"
                dense
                color="#042854"
                dark
                extended
                :extension-height="headerHeight"
                flat
                src="@/assets/images/background.png"
            >
                <v-toolbar-title class="mx-auto text-center" slot="extension">
                    <h2 class="p-6" v-html="$t('home.title')" />
                    <v-text-field
                        class="search-input"
                        filled
                        :placeholder="$t('searchPlaceholder')"
                        append-icon="icofont-search-1"
                        background-color="#fff"
                        dense
                        flat
                        v-model="searchText"
                        @click:append="search()"
                        @keyup.enter="search()"
                    />
                    <div class="flex flex-wrap justify-between" v-if="!isMobile">
                        <h6 :class="{'pr-3': !isMobile}"><baseIcon value="icofont-check" :size="16" color="#62ba2e" />{{ $t('home.seeNutrients') }}</h6>
                        <h6 :class="{'pr-3': !isMobile}">
                            <baseIcon value="icofont-check" :size="16" color="#62ba2e" />{{ $t('home.adjustServings') }}
                        </h6>
                        <h6 :class="{'pr-3': !isMobile}">
                            <baseIcon value="icofont-check" :size="16" color="#62ba2e" />{{ $t('home.changeMeasurements') }}
                        </h6>
                        <h6><baseIcon value="icofont-check" :size="16" color="#62ba2e" />{{ $t('home.makeYourOwnRecipes') }}</h6>
                    </div>
                </v-toolbar-title>
            </v-toolbar>
            <div class="w-full flex flex-wrap justify-center relative">
                <div class="home-navigation-bar" :class="{'home-navigation-bar-mobile': isMobile}" style="margin-top: -30px">
                    <div
                        class="home-navigation-bar__item"
                        v-for="(item, i) in navItems"
                        :key="i"
                        :class="{ active: selectedTagId == item.id }"
                        @click="selectNavItem(item)"
                    >
                        <baseIcon :value="item.icon" v-if="selectedTagId != item.id || isRecipesFetched"></baseIcon>
                        <v-progress-circular
                            v-else
                            :width="1"
                            :size="10"
                            indeterminate
                            color="#3b63d9"
                        ></v-progress-circular>
                        <p>{{ item.name }}</p>
                    </div>
                </div>
            </div>
            <RecipesContent
                ref="RecipesContent"
                paddingTop="30px"
                :sidebarTop="isMobile ? '45px' : '60px'"
                @isRecipesFetched="isRecipesFetched = $event"
                @queriesLoaded="insertQueries($event)"
                :isHome="true"
            />
        </div>
    </baseLayout>
</template>

<script>
import { EventBus } from '@/event-bus'
import _ from "lodash";
export default {
    name: "Home",
    components: {
        AppHeader: () => import("@/components/app/AppHeader.vue"),
        AppHeaderMobile: () => import("@/components/app/AppHeaderMobile.vue"),
        RecipesContent: () => import("@/components/recipes/RecipesContent.vue"),
    },
    data() {
        return {
            changeHeader: false,
            headerHeight: 400,
            isRecipesFetched: false,
            searchText: null,
            selectedTagId: null,
            navItems: [
                { id: null, name: this.$t('tags.all'), icon: "icofont-food-basket", active: true },
                { id: 5, name: this.$t('tags.food'), icon: "icofont-bbq", active: false },
                { id: 943, name: this.$t('tags.drinks'), icon: "icofont-soft-drinks", active: false },
                { id: 61, name: this.$t('tags.desserts'), icon: "icofont-birthday-cake", active: false },
                { id: 604, name: this.$t('tags.condiments'), icon: "icofont-salt-and-pepper", active: false },
                // {id: 'none', name: 'Advanced search', icon: 'icofont-search-restaurant', active: false},
            ],
        };
    },
    computed: {
      isMobile() {
        const bool = window.innerWidth <= 800;
        // eslint-disable-next-line vue/no-side-effects-in-computed-properties
        if(bool) this.headerHeight = 250;
        return bool;
      },
      appStylings() {
        return {
          background: this.changeHeader ? '#fff !important' : 'transparent',
          'box-shadow':  this.changeHeader ? '0px 0px 3px 2px rgb(0 0 0 / 5%)' : 'none'
          // 'border-bottom': this.changeHeader ? '1px solid var(--border-color)' : '0px',
        }
      }
    },
    methods: {
        insertQueries(queries) {
            this.searchText = queries.search;
            this.selectedTagId = queries.tags?.length > 0 ? queries.tags[0] : null;
        },
        selectNavItem(item) {
            if (item.id != "none") {
                this.selectedTagId = item.id;
                if (!item.id) {
                    this.$refs.RecipesContent.queries.tags = "";
                } else {
                    this.$refs.RecipesContent.queries.tags = [item.id];
                }
            }
        },
        search() {
          EventBus.$emit("onSearch", this.searchText);
        },
        goToRecipe(recipe) {
            this.$localeRouter.push({
                name: "Recipe",
                params: { id: recipe.id },
            });
        },
        handleScroll(e) {
          var scrollTop = e?.target?.scrollTop || 0;
          if(scrollTop > (this.headerHeight - 25)) {
            this.changeHeader = true;
          }
          else {
            this.changeHeader = false;
          }
        }
    },
    // watch: {
    //   '$route.query': {
    //     handler(query) {
    //       if(query && !_.isEmpty(query)) {
    //       }
    //     }, immediate: true, deep: true
    //   }
    // }
};
</script>
<style lang="scss">
.home {
    &__body {
        .search-input {
            .v-input__slot {
                border-radius: 0px !important;
                input {
                    padding-left: 5px;
                    color: #4b5b77 !important;
                    font-size: 14px;
                    caret-color: #000;
                }
                input::placeholder {
                    color: #4b5b77 !important;
                    opacity: 1;
                }
                .v-icon {
                    padding-right: 5px;
                    font-size: 18px !important;
                    color: #232844 !important;
                    cursor: pointer;
                }
                &::before {
                    border-style: none !important;
                }
                &::after {
                    border-color: #62ba2e !important;
                }
            }
        }
        .home-navigation-bar {
            display: flex;
            position: absolute;
            z-index: 9;
            background: #f9f9f9;
            border-radius: 25px;
            -webkit-box-shadow: 0px 0px 15px -4px rgba(0, 0, 0, 0.3);
            -moz-box-shadow: 0px 0px 15px -4px rgba(0, 0, 0, 0.3);
            box-shadow: 0px 0px 15px -4px rgba(0, 0, 0, 0.3) !important;
            overflow: hidden;
            background: -webkit-linear-gradient(top, #ffffff 40%, #f1f1f1 80%);
            &.home-navigation-bar-mobile {
              justify-content: space-between;
              width: 100%;
              border-radius: 0px;
              .home-navigation-bar__item {
                min-width: 50px !important;
              }
            }
            &__item {
                padding: 10px 10px;
                cursor: pointer;
                min-width: 125px;
                text-align: center;
                &:hover {
                    background: #fff;
                    transition: all 200ms;
                    .v-icon,
                    p {
                        color: #000;
                    }
                }
                .v-icon {
                    font-size: 20px;
                    color: #555555;
                }
                p {
                    font-size: 12px;
                    color: #858585;
                }
                &.active {
                    .v-icon,
                    p {
                        color: #3b63d9;
                    }
                }
            }
        }
    }
}
</style>
