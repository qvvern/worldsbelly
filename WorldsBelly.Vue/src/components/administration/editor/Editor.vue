<template>
    <div class="editor">
        <v-card v-if="activeIngredient" :class="'edit-ingredient-popup edit-ingredient-popup-'+id" v-click-outside="editIngredientNodeClose">
          <div class="w-full flex flex-col text-center">
            <!-- <p>{{ activeIngredient.ingredient.name }}</p> -->
            <div>
              <baseQuantityInput
                style="margin: 0 auto;"
                :min="1"
                :max="Number(activeIngredient.max)"
                width="150px"
                labelRight
                :value="activeIngredient.amount"
                @input="activeIngredient.newAmount = Number($event)"
              />
            </div>
            <!-- <baseButton dashed @click.prevent.native="editIngredientNodeClose"  noshadow class="mt-2">Update</baseButton> -->
            <!-- <p>{{ get_measurements.find(_ => _.id == activeIngredient.ingredient.measurementId).unit }}</p> -->
          </div>
        </v-card>
        <editor-menu-bar :editor="editor" v-slot="{ commands, isActive, focused }">
            <div class="ml-2 is-hidden" :class="{ 'is-focused': focused || showBar }">
                <div class="menubar">
                    <button class="menubar__button" @click="undoHandler()/*commands.undo*/" :class="{'is-disabled': !undoAvailable}">
                        <baseIcon value="icofont-undo" />
                    </button>

                    <button class="menubar__button" @click="redoHandler()/*commands.redo*/" :class="{'is-disabled': !redoAvailable}">
                        <baseIcon value="icofont-redo" />
                    </button>

                    <button class="menubar__button" :class="{ 'is-active': isActive.bold() }" @click="commands.bold">
                        <baseIcon value="icofont-bold" />
                    </button>

                    <button
                        class="menubar__button"
                        :class="{ 'is-active': isActive.italic() }"
                        @click="commands.italic"
                    >
                        <baseIcon value="icofont-italic" />
                    </button>

                    <button
                        class="menubar__button"
                        :class="{ 'is-active': isActive.strike() }"
                        @click="commands.strike"
                    >
                        <baseIcon value="icofont-strike-through" />
                    </button>

                    <button
                        class="menubar__button"
                        :class="{ 'is-active': isActive.underline() }"
                        @click="commands.underline"
                    >
                        <baseIcon value="icofont-underline" />
                    </button>

                    <!-- <button
                        class="menubar__button"
                        :class="{ 'is-active': isActive.code() }"
                        @click="commands.code"
                      >
                        <baseIcon value="icofont-code" />
                      </button> -->

                    <!-- <button
                        class="menubar__button"
                        :class="{ 'is-active': isActive.paragraph() }"
                        @click="commands.paragraph"
                    >
                        <baseIcon value="icofont-paragraph" />
                    </button> -->

                    <button
                        class="menubar__button"
                        :class="{ 'is-active': isActive.header() }"
                        @click="commands.header({ size: 20 })"
                    >
                        H1
                    </button>

                    <!-- <button
                        class="menubar__button"
                        :class="{ 'is-active': isActive.heading({ level: 2 }) }"
                        @click="commands.heading({ level: 2 })"
                    >
                        H2
                    </button>

                    <button
                        class="menubar__button"
                        :class="{ 'is-active': isActive.heading({ level: 3 }) }"
                        @click="commands.heading({ level: 3 })"
                    >
                        H3
                    </button> -->

                    <button
                        class="menubar__button"
                        :class="{ 'is-active': isActive.bullet_list() }"
                        @click="commands.bullet_list"
                    >
                        <baseIcon value="icofont-listing-box" />
                    </button>

                    <button
                        class="menubar__button"
                        :class="{ 'is-active': isActive.ordered_list() }"
                        @click="commands.ordered_list"
                    >
                        <baseIcon value="icofont-listing-number" />
                    </button>

                    <!-- <button
                        class="menubar__button"
                        :class="{ 'is-active': isActive.blockquote() }"
                        @click="commands.blockquote"
                    >
                        <baseIcon value="icofont-quote-right" />
                    </button> -->

                    <!-- <button
            class="menubar__button"
            :class="{ 'is-active': isActive.code_block() }"
            @click="commands.code_block"
          >
            <baseIcon value="icofont-code-alt" />
          </button> -->

                    <!-- <button class="menubar__button" @click="commands.horizontal_rule">
                        <baseIcon value="icofont-minus" />
                    </button>

                    <button class="menubar__button" @click="commands.alignment({ textAlign: 'left' })">
                        <baseIcon value="icofont-align-left" />
                    </button>
                    <button class="menubar__button" @click="commands.alignment({ textAlign: 'center' })">
                        <baseIcon value="icofont-align-center" />
                    </button>
                    <button class="menubar__button" @click="commands.alignment({ textAlign: 'right' })">
                        <baseIcon value="icofont-align-right" />
                    </button> -->

                    <div>
                        <v-menu v-model="showTextColorPicker" :close-on-content-click="false">
                            <template v-slot:activator="{ on, attrs }">
                                <button
                                    class="menubar__button"
                                    :class="{ 'is-active': showTextColorPicker }"
                                    v-bind="attrs"
                                    v-on="on"
                                >
                                    <baseIcon value="icofont-color-picker" />
                                </button>
                            </template>
                            <div>
                                <color-picker style="box-shadow: 0px" v-model="textColor" class="dvs-mb-2" />

                                <div class="p-2 flex justify-between w-full bg-white">
                                    <baseButton
                                        size="12"
                                        height="30px"
                                        icon="icofont-close"
                                        class="mr-2"
                                        tertiary
                                        @click.prevent.native="showTextColorPicker = false"
                                    >
                                        {{$t('cancel')}}
                                    </baseButton>
                                    <baseButton
                                        white
                                        size="12"
                                        height="30px"
                                        outlined
                                        blue
                                        @click.prevent.native="applyTextColor(commands)"
                                    >
                                        {{$t('apply')}}
                                    </baseButton>
                                </div>
                            </div>
                        </v-menu>
                    </div>
                        <v-menu v-model="showIngredientSelector" :close-on-content-click="false">
                            <template v-slot:activator="{ on, attrs }">
                              <span
                              :style="{'pointer-events': (!ingredientsSelectorItems || ingredientsSelectorItems.length == 0) ? 'none' : 'auto'}"
                              v-bind="attrs"
                                    v-on="on">
                                <baseButton
                                    :disabled="!ingredientsSelectorItems || ingredientsSelectorItems.length == 0"
                                    tertiary
                                    class="menubar__button2"
                                    icon="icofont-ui-add">
                                    {{$t('ingredient')}}
                                </baseButton>
                              </span>
                            </template>
                            <v-card class="add-ingredient-list">
                              <div class="add-ingredient-list__body scrollbar-one">
                                <div class="w-full" v-for="(ingredientList, i) in ingredientsSelectorItems" :key="i">
                                  <div v-if="ingredientsSelectorItems.length > 1">
                                    <h3>{{ ingredientsSelectorItems[i].title }}</h3>
                                  </div>
                                  <v-simple-table class="add-ingredient-list__body-table">
                                    <template v-slot:default>
                                      <tbody>
                                        <tr
                                         v-for="(ingredient, u) in ingredientsSelectorItems[i].ingredients" :key="u"
                                         @click="selectedIngredientId = ingredient.id"
                                         :class="{selected: selectedIngredientId == ingredient.id}"
                                        >
                                          <td>
                                              <p>
                                                {{ ingredient.name }}
                                              </p>
                                          </td>
                                          <td>
                                              <baseQuantityInput
                                                style="margin: 0 auto;"
                                                :min="1"
                                                :max="Number(ingredient.max)"
                                                width="130px"
                                                labelRight
                                                :value="ingredient.max"
                                                @input="ingredientsSelectorItems[i].ingredients[u].amount = Number($event)"
                                              />
                                            </td>
                                        </tr>
                                      </tbody>
                                    </template>
                                  </v-simple-table>
                                </div>
                              </div>
                              <div class="add-ingredient-list__actions flex justify-between items-center p-2">
                                    <v-btn
                                      outlined
                                      small
                                      :size="10"
                                      @click="showIngredientSelector = false, selectedIngredientId = null"
                                    >
                                      {{$t('cancel')}}
                                    </v-btn>
                                    <baseButton
                                      :black="selectedIngredientId ? true : false"
                                      outlined
                                      @click.prevent.native="applyIngredient(commands)"
                                      :disabled="!selectedIngredientId"
                                      :size="12"
                                      icon="icofont-plus" >
                                      {{$t('add')}}
                                    </baseButton>
                              </div>
                            </v-card>
                        </v-menu>
                    </div>
            </div>
        </editor-menu-bar>

        <editor-content class="editor__content" :editor="editor" />
    </div>
</template>
<script>
import { mapGetters } from 'vuex'
import { Editor, EditorContent, EditorMenuBar } from "tiptap";
import AlignText from "@/utils/extensions/tiptap/TextAlign.js";
import TextColor from "@/utils/extensions/tiptap/TextColor.js";
import InsertHtml from "@/utils/extensions/tiptap/InsertHtml.js";
import DataAttributes from "@/utils/extensions/tiptap/DataAttributes.js";
import EnterHandler from "@/utils/extensions/tiptap/EnterHandler.js";
import Header from "@/utils/extensions/tiptap/Header.js";
import { redoUndoMixin } from "@/mixins/redoUndoMixin";
import { EventBus } from '@/event-bus'
// import ResetText from "@/utils/extensions/tiptap/ResetText.js";
import _ from "lodash";

// eslint-disable-next-line no-undef
const Chrome = require(/* webpackChunkName: "vue-color" */ "vue-color/src/components/Chrome.vue").default;

import {
    Blockquote,
    // CodeBlock,
    HardBreak,
    // Heading,
    HorizontalRule,
    OrderedList,
    BulletList,
    ListItem,
    TodoItem,
    TodoList,
    Bold,
    // Code,
    Italic,
    // Link,
    Strike,
    Underline,
    // History,
} from "tiptap-extensions";
export default {
  mixins: [redoUndoMixin],
    props: {
        value: String,
        placeholder: String,
        emitPlacehoder: Boolean,
        showBar: Boolean,
        focused: Boolean,
        ingredientLists: Array,
        rememberIngredients: [Object, Array]
    },
    components: {
        EditorContent,
        EditorMenuBar,
        "color-picker": Chrome,
    },
    data() {
        return {
            id: null,
            editor: null,
            clonedValue: null,
            showTextColorPicker: false,
            showIngredientSelector: false,
            textColor: "#000000",
            // ingredient: null,
            selectedIngredientId: null,
            ingredientsSelectorItems: null,
            recipeContent: null,
            beforeIngredientLists: null,
            activeIngredient: null,
            isFirstLoad: true
        };
    },
    beforeDestroy() {
        this.editor.destroy();
    },
    computed: {
      ...mapGetters('measurement/', ['get_measurements']),
        // ingredientRows() {
        //     var index = 1;
        //     return this.ingredientsSelectorItems.map((ingredientList) => {
        //         return {
        //             title: ingredientList.title,
        //             ingredients: ingredientList.ingredients.map((ingredient) => {
        //               return {

        //               }
        //             }),
        //         };
        //     });
        // },
    },
    methods: {
        applyHeader(commands, size) {
            commands.header({ size: size });
        },
        applyTextColor(commands) {
            this.showTextColorPicker = false;
            if (!this.textColor?.rgba) {
                return;
            }
            const { r, g, b, a } = this.textColor.rgba;
            commands.textcolor({ color: `rgba(${r},${g},${b},${a})` });
        },
        applyIngredient(commands) {
            this.showIngredientSelector = false;
            if(!this.selectedIngredientId) {
              return;
            }
            const ingredient = _.flatMap(this.ingredientsSelectorItems, 'ingredients').find(x => x.id == this.selectedIngredientId);
            commands.insertHTML(
                `&nbsp;<span contenteditable="false" data-ingredient data-uid="${this.$guid.uuidv4()}" data-id="${ingredient.id}" data-amount="${ingredient.amount}">${ingredient.amount}${this.get_measurements.find(_ => _.id == ingredient.measurementId).unit} ${ingredient.name}</span>&nbsp;`
            );
            this.selectedIngredientId = null;
            this.$nextTick(() => {
              EventBus.$emit("onSyncIngredientsEditor");
              EventBus.$emit("onSyncIngredientsSelector");
            })
        },
        handleContentClick(e) {
          if(e?.target?.dataset?.ingredient != null && e.target.dataset.uid != null) {
            var bounds = e.target.getBoundingClientRect();
            let layerX = e.clientX-bounds.left;
            // let layerY = e.clientY-bounds.top;
            let right = bounds.width - layerX;
            if(right < 15) {
              this.removeIngredientNode(e.target.dataset.uid);
            }
            else {
              this.editIngredientNode(bounds, e.target.dataset);
            }
          }
        },
        removeIngredientNode(uid) {
          const comment_json = _.cloneDeep(this.editor.getJSON());
          if (comment_json?.content?.length > 0) {
            comment_json.content = comment_json.content.filter(_ => _.attrs.uid != uid);
            this.editor.setContent(comment_json);
            this.$nextTick(() => {
              this.editorUpdate(this.editor);
              EventBus.$emit("onSyncIngredientsEditor");
              EventBus.$emit("onSyncIngredientsSelector");
            })
          }
        },
        editIngredientNode(bounds, dataset) {
          const ingredients = _.flatMap(this.ingredientsSelectorItems, 'ingredients');
          const allIngredients = _.flatMap(this.ingredientLists, 'ingredients');
          const ingredient = allIngredients.find(_ => dataset.id == _.tempId || dataset.id == _.id);
          const foundIngredient = ingredients.find(_ => ingredient.tempId == _.tempId || ingredient.id == _.id);
          this.activeIngredient = {
            nodeId: dataset.uid,
            ingredient: ingredient,
            amount: dataset.amount,
            max: foundIngredient ? (Number(dataset.amount) + Number(foundIngredient.max)) : 0
          };
            const comment_json = _.cloneDeep(this.editor.getJSON());
            if (comment_json?.content?.length > 0) {
              var foundIndex = comment_json.content.findIndex(_ => _.attrs.uid == this.activeIngredient.nodeId);
              if(foundIndex >= 0) {
                this.$nextTick(() => {
                  var popupEl = document.querySelector('.edit-ingredient-popup-'+this.id);
                  popupEl.style.display = 'block'
                  popupEl.style.top = `${bounds.top + 15}px`;
                  popupEl.style.left = `${bounds.left - 40}px`;
                })
              }
            }
        },
        editIngredientNodeClose(e) {
          if(this.activeIngredient.newAmount != null && this.activeIngredient.amount != this.activeIngredient.newAmount) {
            const comment_json = _.cloneDeep(this.editor.getJSON());
            if (comment_json?.content?.length > 0) {
              var foundIndex = comment_json.content.findIndex(_ => _.attrs.uid == this.activeIngredient.nodeId);
              if(foundIndex >= 0) {
                comment_json.content[foundIndex].attrs.amount = _.clone(this.activeIngredient.newAmount);
                this.editor.setContent(comment_json);
                this.$nextTick(() => {
                  this.editorUpdate(this.editor);
                  this.mapIngredientLists();
                })
              }
            }
          }
          this.activeIngredient = null;
        },
        mapIngredientLists() {
          this.$nextTick(() => {
            // this.updateIngredientsEditor();
            // this.mapIngredientsSelector();
            EventBus.$emit("onSyncIngredientsEditor");
            EventBus.$emit("onSyncIngredientsSelector");
          })
        },
        mapIngredientsSelector: _.debounce(function() {
          this.syncIngredientsSelector();
        }, 100),
        updateIngredientsEditor: _.debounce(function() {
          this.syncIngredientsEditor();
        }, 100),
        syncIngredientsSelector() {
          this.ingredientsSelectorItems = _.cloneDeep(this.ingredientLists).map((list, index) => {
            if(!list || list.ingredients?.length == 0) return;
            list.ingredients = list.ingredients.map(ingredient => {
                if(!ingredient || !ingredient.amount || !ingredient.measurementId) return;
                ingredient.id = ingredient.id || ingredient.tempId;
                ingredient.name = this.rememberIngredients[index]?.find(_ => _.id == ingredient.ingredientId)?.name || ingredient.name;
                var foundSelectors = document.querySelectorAll(`[data-ingredient][data-id="${ingredient.id}"]`);
                if(foundSelectors?.length > 0) {
                  var foundIngredientAmount = parseFloat(_.clone(ingredient.amount));
                  foundSelectors.forEach((e) => {
                    var amountString = e.getAttribute('data-amount');
                    foundIngredientAmount -= parseFloat(amountString);
                  })
                  if(foundIngredientAmount > 0) {
                    ingredient.max = foundIngredientAmount;
                    ingredient.amount = foundIngredientAmount;
                    return ingredient;
                  }
                }
                else {
                  ingredient.max = ingredient.amount;
                  return ingredient;
                }
            });
            if(list?.ingredients?.length > 0) {
             list.ingredients = list.ingredients?.filter(_ => _ != undefined);
             return list
            }
          })?.filter(_ => _ != undefined && _.ingredients.length > 0)
        },
        syncIngredientsEditor() {
            this.$nextTick(() => {
              this.mapIngredientsInEditor();
              this.beforeIngredientLists = _.cloneDeep(this.ingredientLists);
            })
        },
        mapIngredientsInEditor() {
          var isSomeInvalid = false;
          var comment_json = _.cloneDeep(this.editor.getJSON());
          if (comment_json?.content?.length > 0) {
              const ingredients = _.flatMap(this.ingredientLists, 'ingredients');
              comment_json.content = comment_json.content.map((paragraph) => {
                  if (paragraph?.attrs?.ingredient != null) {
                    /* does ingredient exist in list */
                      var foundIngredient = ingredients.find(_ => paragraph.attrs.id == _.tempId || paragraph.attrs.id == _.id)
                      /* no - remove */
                      if(!foundIngredient) return null;
                      /* yes, update content && queryselect all matching */
                      var ingredientName = "";
                          _.cloneDeep(this.ingredientLists).every((list, index) => {
                            var foundname = this.rememberIngredients[index]?.find(_ => _.id == foundIngredient.ingredientId)?.name;
                            if (foundname) {
                              ingredientName = foundname;
                              return false;
                            }
                            return true;
                          });
                          if(!paragraph.content || !paragraph.content[0].text) {
                            paragraph.content = [
                                {
                                    text: "",
                                    type: "text",
                                }
                              ];
                          }
                      paragraph.content[0].text = `${paragraph.attrs.amount}${this.get_measurements.find(_ => _.id == foundIngredient.measurementId).unit} ${ingredientName}`
                      /* does all matching equals same value as in list */
                      var foundSelectors = document.querySelectorAll(`[data-ingredient][data-id="${paragraph.attrs.id}"]`);
                      var totalAmount = 0;
                      foundSelectors.forEach(selector => {
                        totalAmount += Number(selector.getAttribute('data-amount'));
                      })
                      if(totalAmount > Number(foundIngredient.amount)) {
                        paragraph.attrs.invalid = 'true';
                        isSomeInvalid = true;
                      }
                      else {
                        paragraph.attrs.invalid = '';
                      }
                  }
                  return paragraph;
              });
              comment_json.content = comment_json.content.filter(_ => _ != null);
              this.editor.setContent(comment_json);
              this.editorUpdate(this.editor);
              EventBus.$emit("onIsSomeInvalid", {id: this.id, invalid: isSomeInvalid});
          }
        },
        editorUpdate(e) {
                const Vue = this;
                const comment_json = e.getJSON();
                if (comment_json?.content?.length > 0) {
                    var paragraphs = comment_json.content;
                    var updatedParagraphs = [];
                    var emptyParagraph = {
                        attrs: {},
                        content: [
                            {
                                text: " ",
                                type: "text",
                            },
                        ],
                        type: "paragraph",
                    };
                    paragraphs.forEach((paragraph, index) => {
                        if (paragraph?.attrs?.ingredient != null) {
                            var before = paragraphs[index - 1]?.content;
                            if (!before || before.length == 0 || !/\s$/.test(before[before.length - 1].text)) {
                                updatedParagraphs.push(emptyParagraph);
                            }
                            updatedParagraphs.push(paragraph);
                            var after = paragraphs[index + 1]?.content;
                            if (!after || after.length == 0 || !/^\s/.test(after[0].text)) {
                                updatedParagraphs.push(emptyParagraph);
                            }
                        } else {
                            updatedParagraphs.push(paragraph);
                        }
                    });
                    if (paragraphs.length != updatedParagraphs.length) {
                        var newCommentJson = comment_json;
                        newCommentJson.content = updatedParagraphs;
                        e.setContent(newCommentJson);
                        return;
                    }
                }
                const comment_text = e.getHTML();
                if (comment_text == Vue.placeholder) {
                    // Vue.$emit("input", _.cloneDeep(Vue.placeholder));
                    Vue.$emit("input", null);
                } else {
                    var htmlString = _.cloneDeep(comment_text);
                    var htmlObject = document.createElement('div');
                    htmlObject.innerHTML = htmlString;
                    var htmlIngredients = htmlObject.querySelectorAll(`[data-ingredient]`);
                    [].forEach.call(htmlIngredients, function(div) {
                      div.innerHTML = "";
                    });
                    var htmlAll = htmlObject.querySelectorAll(`[data-invalid]`);
                    [].forEach.call(htmlAll, function(div) {
                      div.removeAttribute("invalid");
                    });
                    Vue.$emit("input", htmlObject.innerHTML);
                }
              Vue.mapIngredientsSelector();
              if(Vue.isFirstLoad) {
                  Vue.isFirstLoad = false;
                  Vue.updateRedoUndoTimeout();
              }
              else {
                Vue.updateRedoUndoDebounce();
              }
        },
        updateRedoUndoTimeout() {
          this.$nextTick(() => {
            setTimeout(() => {
              this.updateRedoUndo();
            }, 100);
          })
        },
        updateRedoUndo() {
          const comment_json = _.cloneDeep(this.editor.getJSON());
            if (
              comment_json &&
              !_.isEqual(this.undos[this.undos.length - 1]?.content, comment_json?.content)
            ) {
              this.undos.push(_.cloneDeep(comment_json));
            }
        },
        updateRedoUndoDebounce: _.debounce(function() {
          this.updateRedoUndo();
        }, 100),
        createEditor() {
          const Vue = this;
          this.editor = new Editor({
              extensions: [
                  new Blockquote(),
                  new BulletList(),
                  // new CodeBlock(),
                  new EnterHandler(),
                  new HardBreak(),
                  // new Heading({ levels: [1, 2, 3] }),
                  new Header(),
                  // new ResetText(),
                  new HorizontalRule(),
                  new ListItem(),
                  new OrderedList(),
                  new TodoItem(),
                  new TodoList(),
                  // new Link(),
                  new Bold(),
                  // new Code(),
                  new Italic(),
                  new Strike(),
                  new Underline(),
                  // new History(),
                  new AlignText(),
                  new TextColor(),
                  new InsertHtml(),
                  new DataAttributes(),
              ],
              onFocus() {
                  Vue.mapIngredientsSelector();
              },
              onUpdate() {
                Vue.editorUpdate(this);
              },
              content: this.clonedValue ? this.clonedValue : this.placeholder,
          });
          this.mapIngredientsInEditor();
        },
        clearEditor() {
          this.editor.view.updateState(this.editor.createState())
        },
        keyupHandler (event) {
          if(this.editor.focused) {
            if (event.ctrlKey && event.code === 'KeyZ') {
              this.undoHandler()
            }
            else if (event.ctrlKey && event.code === 'KeyY') {
              this.redoHandler()
            }
          }
        },
    },
    destroyed() {
        document.removeEventListener('keyup', this.keyupHandler)
        this.recipeContent.removeEventListener('click', this.handleContentClick);
    },
    mounted() {
        document.addEventListener('keyup', this.keyupHandler)
        this.id = this._uid
        this.recipeContent = document.querySelector('.recipe-content');
        this.recipeContent.addEventListener('click', this.handleContentClick);
        this.clonedValue = _.cloneDeep(this.value);
        this.createEditor();
    },
    watch: {
      ingredientLists: {
        handler() {
          this.mapIngredientLists()
        }, deep: true, immediate: true
      },
      ingredientsSelectorItems: {
        handler() {
          if(this.ingredientsSelectorItems.length == 0) {
             this.$emit('onAllIngredientsAdded', true);
          }
          else {
             this.$emit('onAllIngredientsAdded', false);
          }
        }, deep: true
      },
      redoUndoItem: {
        handler(redoUndoItem) {
          if(redoUndoItem) {
            this.editor.setContent(redoUndoItem);
            this.$nextTick(() => {
              this.editorUpdate(this.editor);
              EventBus.$emit("onSyncIngredientsEditor");
              EventBus.$emit("onSyncIngredientsSelector");
            })
          }
        },
        deep: true,
      },
    }
};
</script>
<style lang="scss">
.add-ingredient-list {
    &__body {
      overflow: auto;
      max-height: 200px;
      min-height: 50px;
      h3 {
        font-size: 13px;
        padding: 10px;
      }
      &-table {
        tr {
          cursor: pointer;
          &.selected {
            background: #f3f9f4 !important;
          }
        }
      }
    }
    &__actions {
      // height: 50px;
      background: #fafbfc;
      border-top: 1px solid #e6eaea;
    }
  }
  .edit-ingredient-popup {
    display: none;
    position: fixed;
    z-index: 9999999999;
    background: white;
    padding: 8px;
    border: 2px solid rgb(192, 192, 192);
    border-radius: 10px;
    width: 200px;
    top: 0;
    left: 0;
  }
  .menubar {
        user-select: none; /* supported by Chrome and Opera */
        -webkit-user-select: none; /* Safari */
        -khtml-user-select: none; /* Konqueror HTML */
        -moz-user-select: none; /* Firefox */
        -ms-user-select: none; /* Internet Explorer/Edge */
  }
.editor {
    width: 100%;
    .ProseMirror {
        margin: 0px 0px;
        transition: all 500ms;
        padding: 0px 20px 20px 20px;
        outline: none !important;
    }
    [data-ingredient] {
        padding: 0px 5px;
        background: #e9f7fb;
        border: 1px solid #cbe1f5;
        border-radius: 5px;
        user-select: none; /* supported by Chrome and Opera */
        -webkit-user-select: none; /* Safari */
        -khtml-user-select: none; /* Konqueror HTML */
        -moz-user-select: none; /* Firefox */
        -ms-user-select: none; /* Internet Explorer/Edge */
        // pointer-events: none;
        cursor: pointer;
    }
    [data-ingredient]:after {
        content: "\00d7";
        font-weight: normal;
        font-style: normal;
        margin-left: 5px;
        text-decoration: none;
        cursor: pointer;
        pointer-events: all;
    }
    [data-invalid="true"] {
      background: red !important;
    }
}
.vc-chrome {
    box-shadow: none !important;
}
.menubar__button2 {
  margin: 0px !important;
  p, .v-icon {
    font-size: 12px !important;
  }
}
</style>
<style lang="scss" >
.editor {
      .is-disabled {
          pointer-events: none;
          opacity: .7;
      }
      .is-hidden {
          margin-top: -20px;
          padding: 0px !important;
          visibility: hidden;
          opacity: 0;
      }

      .is-focused {
          margin-top: -10px;
          visibility: visible;
          opacity: 1;
          transition: all 500ms;
          padding-top: 0px !important;
          padding: 20px 0px !important;
      }
    .menubar {
        padding: 0px 0px !important;
        // margin-left: -5px !important;
        transition: visibility 0s 0.2s, opacity 0s 0.2s;
        font-size: 18px;
        color: #000;
        line-height: 1.5;
        margin: 0;
        padding: 0;
        box-sizing: border-box;
        text-size-adjust: 100%;
        -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
        -webkit-font-smoothing: antialiased;
        text-rendering: optimizeLegibility;
        transition: all 400ms;
        display: flex;
        &__button {
            font-size: 12px;
            margin: 0;
            box-sizing: border-box;
            text-size-adjust: 100%;
            -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
            -webkit-font-smoothing: antialiased;
            text-rendering: optimizeLegibility;
            font-weight: 700;
            display: inline-flex;
            background: transparent;
            border: 0;
            color: #000;
            padding: 0.2rem 0.5rem;
            margin-right: 0.2rem;
            border-radius: 3px;
            cursor: pointer;
            width: 35px;
            height: 30px;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            &.is-active {
                background-color: rgba(0, 0, 0, 0.1);
            }
            &:hover {
                background-color: rgba(0, 0, 0, 0.1);
            }
            .base-icon {
                font-weight: 700;
                color: #000;
                cursor: pointer;
                font-size: 14px;
                margin: 0;
                padding: 0;
            }
        }
    }
}
</style>
