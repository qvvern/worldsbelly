/* eslint-disable class-methods-use-this */
import { Mark } from 'tiptap'
import { toggleMark } from 'tiptap-commands'

export default class ResetText extends Mark {

  get name () {
    return 'resetText'
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
        tag: `span[style=""`,
        attrs: { size },
      })),
      toDOM:
        node => {
          return ['span', {
            style: ``
          }, 0]
        }
    }
  }

  commands ({ type }) {
    return (attrs) => toggleMark(type, attrs)
  }
}
