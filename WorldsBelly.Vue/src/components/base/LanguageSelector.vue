<template>
  <div class="language-selector" ref="language-selector">
    <div class="select_component" :class="{active: isOpen}">
      <div class="select_component__selected" @click.prevent="isOpen = !isOpen" :class="{'home-mb': isHome && isMobile}">
        {{ $i18n.locale.toUpperCase() }}
      </div>
      <ul class="select_component__options" v-if="isOpen">
        <li
          v-for="lang in $languages"
          :key="lang.code"
          @click.prevent="select(lang)"
          :class="{selected: $i18n.locale == lang.code}">
            <b class="pr-2"> {{ lang.nativeName }} </b> ({{ lang.englishName }})
          </li>
      </ul>
    </div>
      <!-- <select v-model="$i18n.locale" class="select_component">
        <option
          v-for="lang in get_languages"
          :key="lang.code"
          :value="lang.code"
          :label="lang.code.toUpperCase()">
            {{ lang.code }}
        </option>
      </select> -->
  </div>
</template>


<script>
import { mapGetters } from "vuex";

export default {
    name: "LanguageSelector",
    props: {
        isHome: Boolean,
    },
    data() {
      return {
        isOpen: false,
      };
    },
    computed: {
        ...mapGetters("language/", ["get_languages"]),
      isMobile() {
        return window.innerWidth <= 800;
      },
    },
    methods: {
      select(lang) {
        const path = this.$route.fullPath;
        if(path && path.startsWith(`/${this.$language.code}/`)) {
          const newPath = path.replace(`/${this.$language.code}/`, `/${lang.code}/`);
          window.location = location.origin + newPath;
        }
        else {
          window.location = location.origin + `/${lang.code}` + path;
        }
        this.close();
      },
      close() {
        this.isOpen = false;
      },
      documentClick(e) {
        if (this.isOpen) {
          const el = this.$refs["language-selector"];
          const { target } = e;
          if (el !== target && !el.contains(target)) {
            this.isOpen = false;
          }
        }
      },
    },
    created() {
      document.addEventListener("mousedown", this.documentClick);
    },
    destroyed() {
      document.removeEventListener("mousedown", this.documentClick);
    },
};
</script>

<style lang="scss" scoped>
@media only screen and (max-width: 800px) {
  .language-selector {
    .select_component {
      &__selected {
        background-color: transparent !important;
        border: 2px solid transparent !important;
        padding: 0px !important;
        margin-right: 0px !important;
        &.home-mb {
          color: #fff !important;
        }
      }
    }
  }
}
.language-selector {
  .select_component {
    position: relative;
    &:hover, &.active {
      .select_component__selected {
        border: 2px solid #fff;
      }
      .select_component__selected, .select_component__options {
        background-color: rgb(233, 231, 231);
      }
    }
    &__selected {
      display: flex;
      align-items: center;
      height: 30px;
      justify-content: space-between;
      background-color: #fafafa;
      border-radius: 8px;
      border: 2px solid #e9e9e9;
      font-size: 13px;
      padding: 6px 8px;
      cursor: pointer;
      margin-right: 8px;
      font-weight: 500;
    }
    &__options {
      margin-left: 2px;
      margin-top: -6px;
      border-bottom-left-radius: 8px;
      border-bottom-right-radius: 8px;
      position: absolute;
      z-index: 9999;
      left: -50%;
      list-style-type: none;
      padding-left: 0px;
      background-color: #fff;
      -webkit-box-shadow: 0 0 10px rgba(0,0,0,0.2);
      -moz-box-shadow: 0 0 10px rgba(0,0,0,0.2);
      box-shadow: 0 0 10px rgba(0,0,0,0.2);
      li {
        font-size: 13px;
        padding: 6px 8px;
        cursor: pointer;
        display: flex;
        align-items: center;
        &:hover {
          background-color: #fff;
        }
        &.selected {
          background-color: rgb(196, 196, 196);
        }
      }
    }

  }
}
</style>
