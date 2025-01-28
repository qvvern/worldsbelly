<template>
    <div class="app-bar flex justify-between items-center" v-if="!isLoginPage">
        <div class="app-bar-logo" @click="$localeRouter.push('/home')">
            <img :src="logo" alt="Logo for worlds belly" />
        </div>
        <div class="app-bar-search" v-if="!isHome">
            <BaseSearchInput
                :placeholder="$t('searchPlaceholder')"
                small
                outlined
                v-model="searchText"
                @enter="search()"
            />
            <!-- <baseInput
            >
              <template slot="prepend">
                  <baseIcon class="cursor-pointer" @click.prevent.native="search()" style="padding-top: 2px; padding-right: 3px" value="icofont-search-1" :size="20" color="#62ba2e" />
              </template>
            </baseInput> -->
        </div>
        <div class="app-bar-actions flex flex-auto items-center justify-end">
            <LanguageSelector :isHome="isHome" />
            <BaseUiButton
                @click.prevent.native="$localeRouter.push({ name: 'RecipeCreate' })"
                :size="12"
                type="btn4"
                class="ml-4"
                :style="{background: !isHome ? null : 'transparent'}"
                icon="bi-plus-circle"
            >
                {{ $t('makeRecipe') }}
            </BaseUiButton>
            <v-divider
                class="mx-5"
                style="height: 35px"
                vertical
                :style="{ 'border-color': !isHome ? null : 'rgba(255,255,255,.6)' }"
            />
            <div class="mr-3 ml-2" v-if="!$msal.isAuthenticated && !isLoginPage">
                <BaseUiButton
                    type="btn2"
                    @click.prevent.native="signIn"
                    :size="14"
                >
                  {{ $t('signIn') }}
                </BaseUiButton>
            </div>
            <AppHeaderNotifications :isHome="isHome" class="mr-5" v-if="$msal.isAuthenticated"/>
            <AppHeaderProfile class="mr-4" v-if="$msal.isAuthenticated" />
        </div>
    </div>
</template>

<script>
import { mapGetters } from "vuex";
import { EventBus } from '@/event-bus'

export default {
    name: "appHeader",
    props: {
        isHome: Boolean,
    },
    data: () => {
        return {
            searchText: null,
        };
    },
    components: {
        AppHeaderProfile: () => import("@/components/app/AppHeaderProfile.vue"),
        AppHeaderNotifications: () => import("@/components/app/AppHeaderNotifications.vue"),
        LanguageSelector: () => import("@/components/base/LanguageSelector.vue"),
    },
    computed: {
        ...mapGetters("recipe/", ["get_recipes_filter"]),
        isLoginPage() {
          return this.$route.name == 'Login';
        },
        logo() {
            return require("@/assets/images/logo.svg");
        },
    },
    methods: {
        signIn() {
            this.$router.push({ name: "Login", query: { redirect: this.$route.path } });
        },
        search() {
          EventBus.$emit("onSearch", this.searchText);
        }
    },
    watch: {
      'get_recipes_filter.search': {
        handler(search) {
          if(this.searchText != search) {
            this.searchText = search;
          }
        }, deep: true, immediate: true
      }
    }
};
</script>

<style lang="scss" scoped>
.app-bar {
    position: relative;
    width: 100%;
    height: 60px;
    z-index: 9;
    background: transparent !important;
    transition: all 200ms;
    box-shadow: 0px 0px 3px 2px rgb(0 0 0 / 5%);
    // border-bottom: 1px solid var(--border-color);
    &-logo {
        opacity: 1;
        height: 60px;
        display: flex;
        align-items: center;
        padding: 18px;
        margin-right: 20px;
        img {
            height: 100%;
        }
    }
    &-search {
        width: 500px;
        display: flex;
    }
    &-actions {
        // min-width: 200px;
    }
}
</style>
