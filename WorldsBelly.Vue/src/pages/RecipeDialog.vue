<template>
    <div class="recipe-dialog">
        <transition name="fade">
            <div v-if="show" class="recipe-dialog-overlay" ref="overlay" @click.stop.self="onClose()" tabindex="0">
                <div class="recipe-dialog-close" @click="onClose()">
                    <baseIcon value="icofont-close" :size="20" />
                </div>
            </div>
        </transition>
        <transition name="slide-up">
            <div v-show="show" class="recipe-dialog-content">
                <RecipeContent v-if="!isMobile" inDialog/>
                <RecipeContentMobile v-else inDialog/>
            </div>
        </transition>
    </div>
</template>

<script>
import { mapMutations, mapGetters, mapActions } from "vuex";
// import findIndex from "lodash/findIndex";

export default {
    name: "RecipeDialog",
    components: {
        RecipeContent: () => import("@/components/recipe/RecipeContent.vue"),
        RecipeContentMobile: () => import("@/components/recipe/RecipeContentMobile.vue"),
    },
    data() {
        return {
            show: false,
            fromRoute: null,
            isLoading: true,
        };
    },
    computed: {
        ...mapGetters("recipe/", ["get_recipe", "get_recipes"]),
        isMobile() {
          return window.innerWidth <= 800;
        }
    },
    methods: {
        ...mapActions("recipe/", ["fetch_recipe_local", "fetch_append_recipes"]),
        ...mapMutations("recipe/", ["set_recipe"]),
        onClose() {
            this.show = false;
            setTimeout(() => {
                if (this.fromRoute) {
                    return this.$router.replace(this.fromRoute.fullPath);
                }
                this.$router.replace("./");
            }, 450);
        },
    },
    mounted() {
        this.show = true;
        this.$nextTick(() => {
            this.$refs.overlay.focus();
        });
    },
    created() {
        const recipeKey = this.$route.params.id;
        this.isLoading = true;
        var fetch_recipe_local = this.fetch_recipe_local(recipeKey);
        Promise.all([fetch_recipe_local]).finally(() => {
            this.isLoading = false;
        });
    },
    beforeRouteEnter(to, from, next) {
        next((vm) => {
            vm.fromRoute = from;
        });
    },
    watch: {
        $route(to, from) {
            const recipeKey = to.params.id;
            this.fetch_recipe_local(recipeKey);
        },
    },
};
</script>

<style lang="scss" scoped>
.recipe-dialog {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    z-index: 999;

    &-overlay {
        background-color: rgba(#000, 0.8);
        position: fixed;
        top: 0;
        left: 0;
        bottom: 0;
        right: 0;
        width: 100%;
        height: 100%;
        z-index: 300;
    }

    &-close {
        position: absolute;
        top: 3px;
        right: 8px;
        height: 30px;
        width: 30px;
        border-radius: 50px;
        cursor: pointer;

        i {
            font-size: 13px;
            font-weight: 300;
            color: #dbdbde;
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
        }

        &:hover {
            i {
                transition: color 0.2s;
                color: #fff;
            }
        }
    }

    .recipe-dialog-content {
        position: absolute;
        top: 35px;
        left: 0;
        bottom: 0;
        right: 0;
        width: 100%;
        height: calc(100% - 35px);
        width: auto;
        margin: 0 auto;
        z-index: 1002;
    }
}

.slide-up-enter {
    opacity: 0.8;
    -webkit-transform: translate(0, 100px);
    transform: translate(0, 100px);
}

.slide-up-leave-to {
    opacity: 0.1;
    -webkit-transform: translate(0, 100px);
    transform: translate(0, 100px);
}

.slide-up-enter-active {
    //   transition: all .3s ease;
    transition: all 0.4s cubic-bezier(0.25, 0.1, 0.25, 1);
}

.slide-up-leave-active {
    transition: all 0.3s cubic-bezier(0.25, 0.1, 0.25, 1);
}

.fade-enter-active,
.fade-leave-active {
    transition: opacity 0.4s;
}
.fade-enter,
.fade-leave-to {
    opacity: 0;
}
</style>
