import { Node } from 'tiptap'
import { setBlockType } from 'tiptap-commands'

export default class Paragraph extends Node {

    get name() {
        return 'paragraph'
    }

    get schema() {
        return {
            attrs: {
                ingredient: {
                    default: null
                },
                id: {
                    default: null
                },
                uid: {
                  default: null
                },
                amount: {
                    default: null
                },
                contenteditable: {
                  default: null
                },
                invalid: {
                  default: null
                },
            },
            content: 'inline*',
            group: 'block',
            draggable: false,
            parseDOM: [{
                tag: 'span',
                getAttrs(dom) {
                    return { ingredient: dom.dataset.ingredient, id: dom.dataset.id, uid: dom.dataset.uid, amount: dom.dataset.amount, invalid: dom.dataset.invalid, contenteditable: dom.getAttribute('contenteditable') };
                }
            }],
            toDOM: node => ['span', { 'data-ingredient': node.attrs.ingredient, 'data-id': node.attrs.id, 'data-uid': node.attrs.uid, 'data-amount': node.attrs.amount, 'data-invalid': node.attrs.invalid, "contenteditable": node.attrs.contenteditable }, 0],
        }
    }

    commands({ type }) {
        return () => setBlockType(type)
    }
}
