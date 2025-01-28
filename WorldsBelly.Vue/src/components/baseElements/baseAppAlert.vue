<template>

    <v-snackbar
      top
      center
      color="primary"
      text
      v-model="snackbar"
      :timeout="timeout"
      elevation="24"
      >
      <span class="flex justify-between items-center">
        {{ message }}
        <BaseIcon value="icofont-close-line" class="cursor-pointer" @click.prevent.native="removeAppAlert" />
      </span>
    </v-snackbar>
</template>

<script>
import { mapActions } from "vuex";

export default {
    name: "baseAppAlert",
    props: {
        type: [String, Number],
        index: [String, Number],
        message: [String, Number],
        delay: [String, Number],
    },
    data() {
        return {
            snackbar: true,
            timeout: 2500,
        };
    },
    created() {
        if (this.type === "error") {
            this.timeout = 4500;
        }
        if (this.delay) {
            this.timeout = this.delay;
        }

        setTimeout(() => {
            this.removeAppAlert();
        }, this.timeout + 10);
    },
    methods: {
        ...mapActions("app/", ["remove_app_aplert"]),

        removeAppAlert() {
            this.remove_app_aplert(this.index);
        },
    },
};
</script>
