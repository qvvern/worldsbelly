<template>
    <v-dialog
      v-model="isOpen"
      :width="width"
      :fullscreen="$vuetify.breakpoint.xsOnly"
    >
      <v-card>
        <v-card-title class="header text-center">
            <span class="text-center w-full"> <baseIcon value="icofont-info-circle" color="#eb3800" class="m-3" size="28" /></span>
            <span class="text-center w-full" v-html="title || $t('actionCannotBeUndone')"></span>
        </v-card-title>
        <v-card-text class="body text-center">
          <template v-if="$slots.default">
            <slot></slot>
          </template>
          <template v-else>
            <span v-html="description || $t('areYouSureYouWantToContinue')"></span>
          </template>
          <template>
            <div>
             <baseButton tertiary style="height: 40px" :loading="isSending" @click.prevent.native="sendVerificationCode()" class="mt-2" noshadow>{{$t('send')}} {{$t('verificationCode')}}</baseButton>
            <p style="font-size: 12px;color: red;">{{confirmEmailStatus}}</p>
             <v-text-field class="mx-6 mt-6" :disabled="!confirmEmailStatus" dense v-model="verificationCode" :label="$t('verificationCode')" :rules="[rules.required]" />

            <p class="mt-6" style="font-size: 12px;color: red;" v-if="deleteProfileStatus">{{deleteProfileStatus}}</p>
            </div>
          </template>
        </v-card-text>
        <v-card-actions class="footer text-center">
            <span class="text-center w-full mb-4">
                <baseButton tertiary large @click.prevent.native="cancel" class="mr-3">{{ cancelBtn || $t('no') }}</baseButton>
                <baseButton tertiary black large @click.prevent.native="deleteProfile" :disabled="!verificationCode || isDeleting" :loading="isDeleting">{{ confirmBtn || $t('yes') }}</baseButton>
            </span>
        </v-card-actions>
      </v-card>
    </v-dialog>
</template>

<script>
import { mapActions } from 'vuex'
  export default {
      name: "ProfileDeleteDialog",
      props: {
        value: [Boolean],
        width: {
          type: String,
          default: "400",
        },
        title: {
          type: String,
          default: null,
        },
        cancelBtn: {
          type: String,
          default: null,
        },
        confirmBtn: {
          type: String,
          default: null,
        },
        description: {
          type: String,
          default: null,
        },
      },
    data() {
      return {
        isOpen: false,
        isSending: false,
        verificationCode: null,
        confirmEmailStatus: null,
        deleteProfileStatus: null,
        isDeleting: false,
          rules: {
            required: v => !!v || this.$t('fieldIsRequired'),
          },
      }
    },
    computed: {
        classObject() {
          return {
            // primary: this.primary,
            // secondary: this.secondary,
          }
        },
        msalUser() {
            return this.$msal.getProfile()?.idTokenClaims;
        },
        email() {
            return this.msalUser?.emails[0];
        },
    },
  methods: {
    ...mapActions('user/', ['send_delete_profile_verification_code', 'delete_profile']),
    open() {
      this.isOpen = true;
    },
    close() {
      this.isOpen = false;
    },
    confirm() {
        this.$emit('confirm', true);
    },
    cancel() {
        this.$emit('cancel', true);
        this.close();
    },
    sendVerificationCode() {
          this.isSending = true;
          this.send_delete_profile_verification_code().then(() => {
            this.confirmEmailStatus = `${this.$t('emailVerificationSentTo')} ${this.email}. ${this.$t('pleaseCheckSpamFolder')}`
          }).catch((e) => {
            this.confirmEmailStatus = e
          }).finally(() => {
            this.isSending = false;
          })
    },
    deleteProfile() {
      this.isDeleting = true;
      this.deleteProfileStatus = null;
      this.delete_profile(this.verificationCode).then(() => {
        this.close();
      }).catch((e) => {
            this.deleteProfileStatus = e
      }).finally(() => {
        this.isDeleting = false;
      });
    },
  },
  watch: {
    value: {
      immediate: true,
      handler(oldVal, newVal) {
        if(oldVal == newVal) return;
        if(this.value != null) this.isOpen = this.value;
      },
    },
  }
  };
</script>
<style lang="scss" scoped>
</style>
