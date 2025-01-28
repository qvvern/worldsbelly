<template>
    <v-menu
      v-model="menu"
      transition="slide-y-transition"
        open-on-click
        bottom
        left
        :close-on-content-click="true"
        :z-index="999999"
        >
        <template v-slot:activator="{ on, attrs }">
            <div
                class="pb-0 cursor-pointer social-share-btn"
                v-bind="attrs"
                v-on="on"
            >
                <slot></slot>
            </div>
        </template>
        <v-card>
          <div class="social-menu">
          <template v-for="(btn, i) in btns">
            <component :is="btn" :key="i"
            class="share-button--outline"
            :url="getUrl"
            :description="text" />
          </template>
          </div>
        </v-card>
    </v-menu>
</template>

<script>
import TwitterButton from "vue-share-buttons/src/components/TwitterButton";
import WhatsAppButton from "vue-share-buttons/src/components/WhatsAppButton";
import FacebookButton from "vue-share-buttons/src/components/FacebookButton";
import LinkedInButton from "vue-share-buttons/src/components/LinkedInButton";
import LiveJournalButton from "vue-share-buttons/src/components/LiveJournalButton";

export default {
    name: "SocialBtn",
    components: {
      TwitterButton,
      WhatsAppButton,
      FacebookButton,
      LinkedInButton,
      LiveJournalButton
    },
    props: {
      title: String,
      text: String,
    },
    data() {
        return {
          menu: false,
          btns: [
            'FacebookButton', 'TwitterButton', 'WhatsAppButton', 'LinkedInButton', 'LiveJournalButton'
          ]
        };
    },
    mounted() {
    },
    computed: {
      getUrl() {
        return document.location.href;
      }
    },
    methods: {
    },
    watch: {
    }
};
</script>
<style lang="scss">
.social-menu {
  display: flex;
  flex-direction: column;
}
.social-share-btn {
    &:hover {
      .v-icon {
        color: #ee3529 !important;
      }
    }
}
</style>
