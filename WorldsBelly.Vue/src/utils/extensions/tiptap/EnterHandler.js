import { Extension, Plugin } from 'tiptap'
import { DOMParser } from 'prosemirror-model'

export default class EnterHandler extends Extension {

  get name() {
    return 'enter_handler'
  }

  get plugins() {
    return [
      new Plugin({
        props: {
          handleKeyDown: (view, event) => {
            if (event.key === 'Enter' && !event.shiftKey) {
              // do something
              // console.log('trig', view);
              // const { selection } = view.state
              // const element = document.createElement('div')
              // element.innerHTML = `<span data-break></span>`;
              // const slice = DOMParser.fromSchema(view.state.schema).parseSlice(element)
              // const transaction = view.state.tr.insert(selection.anchor, slice.content)
              // console.log(transaction);
              // view.dispatch(transaction);
              // const pos = view.state.selection.$anchor.pos;
              // console.log(this.editor);
              if(!this.editor.isActive.bullet_list() && !this.editor.isActive.ordered_list()) {
                this.editor.commands.hard_break();
                return true;
              }
            }
          },
        },
      }),
    ]
  }

}

