<template>
  <div class="base-contenteditable flex items-start">
    <div ref="baseContenteditable"
            :class="{hover: hover}"
            @mouseover="hover = true"
            @mouseleave="hover = false">
      <slot></slot>
    </div>
    <baseIcon
            v-if="!hideIcon"
            :style="styleObject"
            :color="light ? 'white' : 'black'"
            @mouseover.prevent.native="hover = true"
            @mouseleave.prevent.native="hover = false"
            @click.prevent.native="focusEditableContent()" size="14" class="pl-1 pb-1 cursor-pointer">mdi-pencil</baseIcon>
  </div>
</template>

<script>
  export default {
      name: "baseContenteditable",
      props: {
        value: [String, Number],
        disabled: [Boolean],
        correct: [Boolean],
        incorrect: [Boolean],
        limit: [Number],
        light: [Boolean],
        top: [String, Number],
        nullable: [Boolean],
        number: [Boolean],
        placeholder: [String],
        hideIcon: {
          type: Boolean,
          default: false,
        },
        emitPlaceholder: {
          type: Boolean,
          default: false,
        }
      },
      computed: {
        styleObject() {
          return {
            'padding-top': this.top+'px',
          }
        },
      },
    data() {
      return {
        hover: false,
      }
    },
    methods: {
      focusEditableContent() {
        const { baseContenteditable } = this.$refs;
        baseContenteditable.firstChild.focus();
      },
      focusEvent(e, pos) {
        var range = document.createRange();
        range.selectNodeContents(e.target);
        var sel = window.getSelection();
        if(pos) {
          range.setStart(e.target.childNodes[0], pos);
          range.collapse(true);
        }
        sel.removeAllRanges();
        sel.addRange(range);
      },
      updateEditable(e) {
        // console.log(e.target.innerText)
        if(e.target.innerText.length > 0) {
          this.$emit('input', e.target.innerText);
          e.target.innerText = e.target.innerText.toString();
        }
        else if(!this.nullable) {
          e.target.innerText = this.value.toString();
        }
        else {
          this.$emit('input', null);
          e.target.innerText = this.placeholder.toString();
        }
      },
      clearFormatting(e) {
        e.preventDefault();
        if(this.number && isNaN(e.clipboardData.getData('text/plain'))) return;
        var selectedText = window.getSelection().toString();
        if (selectedText) {
          e.target.innerText = e.target.innerText.replace(selectedText, e.clipboardData.getData('text/plain'));
          this.validate(e);
          return;
        }
        const prevPos = this.getCaretPosition(e.target);
        var text = e.target.innerText;
        var firstText = text.slice(0, prevPos);
        var secondText = text.slice(prevPos);
        e.target.innerText = firstText + e.clipboardData.getData('text/plain') + secondText;
        this.validate(e);
      },
      validate(e) {
        if (e.key === 'Enter') {
          e.preventDefault();
          return;
        }
        if(this.number) {
          if (isNaN(String.fromCharCode(e.which))) e.preventDefault();
        }
        if(this.limit && e.target.innerText.length >= this.limit) {
          const prevPos = this.getCaretPosition(e.target)
          e.target.innerText = this.$string.maxLength(e.target.innerText.toString(), this.limit);
          if(prevPos >= this.limit) {
            this.focusEvent(e, this.limit);
          }
          else {
            this.focusEvent(e, prevPos);
          }
        }
      },
      getCaretPosition(editableDiv) {
        var caretPos = 0,
          sel, range;
        if (window.getSelection) {
          sel = window.getSelection();
          if (sel.rangeCount) {
            range = sel.getRangeAt(0);
            if (range.commonAncestorContainer.parentNode == editableDiv) {
              caretPos = range.endOffset;
            }
          }
        } else if (document.selection && document.selection.createRange) {
          range = document.selection.createRange();
          if (range.parentElement() == editableDiv) {
            var tempEl = document.createElement("span");
            editableDiv.insertBefore(tempEl, editableDiv.firstChild);
            var tempRange = range.duplicate();
            tempRange.moveToElementText(tempEl);
            tempRange.setEndPoint("EndToEnd", range);
            caretPos = tempRange.text.length;
          }
        }
        return caretPos;
      }
    },
    mounted() {
      this.$nextTick(() => {
        const { baseContenteditable } = this.$refs;
        if(!baseContenteditable.firstChild) {
          baseContenteditable.innerText = "ERROR";
          return;
        }
        if(this.limit) {
          baseContenteditable.firstChild.innerText = this.$string.maxLength(this.value ? this.value.toString() : this.placeholder.toString(), this.limit);
        }
        else {
          baseContenteditable.firstChild.innerText = this.value ? this.value.toString() : this.placeholder.toString();
        }
        baseContenteditable.firstChild.setAttribute("contenteditable", "true");
        baseContenteditable.firstChild.setAttribute("data-id", this.id);
        baseContenteditable.firstChild.addEventListener('focus', (event) => this.focusEvent(event));
        baseContenteditable.firstChild.addEventListener('blur', (event) => this.updateEditable(event));
        baseContenteditable.firstChild.addEventListener('keypress', (event) => this.validate(event));
        baseContenteditable.firstChild.addEventListener('paste', (event) => this.clearFormatting(event));
        if(this.emitPlaceholder && !this.value) this.$emit('input', this.placeholder.toString());
      });
    },
    beforeDestroy() {
        const { baseContenteditable } = this.$refs;
        baseContenteditable.firstChild.removeEventListener('focus', (event) => this.focusEvent(event));
        baseContenteditable.firstChild.removeEventListener('blur', (event) => this.updateEditable(event));
        baseContenteditable.firstChild.removeEventListener('keypress', (event) => this.validate(event));
        baseContenteditable.firstChild.removeEventListener('paste', (event) => this.clearFormatting(event));
    },
  };
</script>
<style lang="scss" scoped>
.base-contenteditable {
  outline: none;
  *, :focus {
    outline: none;
  }
  .hover * {
    background: rgba(120, 248, 0, 0.1);
    border-radius: 2px;
  }
}
</style>
