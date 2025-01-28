/* eslint-disable class-methods-use-this */
import { Mark } from 'tiptap'
import { toggleMark } from 'tiptap-commands'

export default class Header extends Mark {

  get name () {
    return 'header'
  }

  get defaultOptions () {
    return {
      size: ['14px'],
    }
  }

  get schema () {
    return {
      attrs: {
        size: {
          default: '14',
        },
      },
      parseDOM: this.options.size.map(size => ({
        tag: `span[style="font-size:${size};font-weight:bold"]`,
        attrs: { size },
      })),
      toDOM:
        node => {
          return ['span', {
            style: `font-size:${node.attrs.size}px;font-weight:bold`
          }, 0]
        }
    }
  }

  commands ({ type }) {
    return (attrs) => toggleMark(type, attrs)
  }
}
