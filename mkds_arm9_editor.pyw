import io
import os
import re
import json
import base64
import webbrowser
import tkinter as tk
from tkinter import filedialog, messagebox, ttk

from PIL import Image, ImageTk

ARM9_BIN_PATH = None
ARM_VALUES = []
with open('music_offsets.json', 'r') as file:
    MUSIC_OFFSETS = json.load(file)
with open('course_offsets.json', 'r') as file:
    COURSE_OFFSETS = json.load(file)
with open('slot_offsets.json', 'r') as file:
    SLOT_OFFSETS = json.load(file)
with open('emblem_offsets.json', 'r') as file:
    EMBLEM_OFFSETS = json.load(file)
ICON_BASE64 = """AAABAAEAEBAAAAEAIABoBAAAFgAAACgAAAAQAAAAIAAAAAEAIAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAzIxH/MyIQ/zIjEP8AAQEAAQAAAAABAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAAQAAAQAAMyMR/zIjEP8yIhD/NykW/0Y6Lf96c2//e3Jv/3tzb/97c2//e3Nu/3tzb/97cm//enNu/3pzb/97c27/e3Nu/3pyb/9GOiz/NygX/0U6I/+GhID/xMTO/+vr7f/39vf/9/b2//b39//29/f/9/f2//f39//29/b/9/b3/+vr7f/Fxc7/h4SB/0U7Iv9KQyn/SkIp/5eUk/92bmf/JSVP/wEJe/8ZFUf/GRVG/xkURv8ZFEb/AQh6/yQlT/92b2f/l5WS/0pDKf9KQij/VlMt/2NjMP9iYzH/MSAQ/xkVR/8BAcb/SEjd/25v3f9ub9z/SUnc/wEBxv8ZFUf/MCEQ/2JiMf9jYjH/V1Is/1hSQP9nY1f/Z2NW/0tDJf8+Mh3/Fxfe/4eG5f9UVOf/VFXn/4eH5P8WF97/PzId/0pDJf9nYlf/ZmJX/1lTQP90dFv/dXRa/3R0W/9lYkT/ZWJF/y0xuP+Agff/gID2/4GB9/+Bgff/LTC5/2VjRP9lYkX/dHVb/3R1W/90dFv/AAEBAAABAAAAAAAAoKGh/52clP/Awcf/VkpB/1IgX/9TIV//VkpB/8HAxv+cnJT/oKCh/wAAAAAAAAAAAAAAAAABAAAAAAAAAAAAAKKjqf+am57/vr++/3R+c/9LW2//Slpv/3R+cv++vr7/m5ue/6Ojqf8AAQAAAAABAAEAAQAAAAAAAAEAAAABAQDc3eD/3N3g/0tqnP9ERkf/TFlt/0xZbf9ERkf/S2qc/93d4f/d3eH/AAAAAAAAAAAAAAAAAAEAAAEBAAAAAQAAAAAAAAEAAABff6L/fp/M/2aIs/9niLP/hKnY/19+o/8AAAEAAAEAAAAAAAAAAAAAAAEAAAABAAAAAQAAAAEBAAABAQAAAAEADQ5h/4OjzP99fYD/fHyA/4KizP8NDmD/AAAAAAEBAAAAAQAAAAAAAAAAAQABAAAAAAEAAAAAAAABAQAAAAAAAAAEmP8NDmD/JC9u/yQvbv8MD2D/AAWY/wEAAAAAAQAAAAAAAAAAAAABAAAAAAAAAAEBAAAAAQAAAQABAAAAAAAAAMv/AQC9/xYasf8XGrD/AQC9/wAAy/8AAAAAAAEAAAABAQAAAAAAAAAAAAAAAQAAAAAAAQAAAAAAAAABAAAAAQABAERF7/+Tkuf/kpPn/0RF7/8AAAAAAAAAAAABAAAAAAAAAAEAAAAAAAABAAAAAQAAAAEBAAABAAEAAAAAAAEBAAAAAAAAWlv3/1ta9v8AAAEAAAEBAAABAAAAAAEAAAEAAAEAAAABAAEAH/gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAOAHAADgBwAA4AcAAPgfAAD4HwAA+B8AAPgfAAD8PwAA/n8AAA=="""


def get_icon_from_base64(base64_string):
    icon_data = base64.b64decode(base64_string)
    icon = Image.open(io.BytesIO(icon_data))
    return ImageTk.PhotoImage(icon)


def read_file(filename):
    try:
        with open(filename, 'rb') as file:
            return file.read()
    except Exception as e:
        messagebox.showerror("Error",
                             f"Failed to open file: {filename}\n\nError: {e}")


def open_file():
    global ARM_VALUES, ARM9_BIN_PATH
    file_path = filedialog.askopenfilename(title="Select ARM9 Binary File",
                                           filetypes=(("Binary Files", "*.bin"), ("All Files", "*.*")))
    if file_path:
        ARM9_BIN_PATH = file_path
        if os.path.getsize(ARM9_BIN_PATH) == 0:
            messagebox.showerror("Error", "This is not an arm9.bin file.")
            return
        file_content = read_file(ARM9_BIN_PATH)
        if file_content:
            ARM_VALUES = [int(byte) for byte in file_content]
            if not ARM_VALUES:
                messagebox.showerror("Error", "This is not an arm9.bin file.")
            else:
                refresh_music_listbox()
                refresh_file_listbox()
                refresh_slot_listbox()
                refresh_emblem_listbox()


def save_file(file_path=None):
    global ARM_VALUES, ARM9_BIN_PATH
    if not ARM9_BIN_PATH:
        messagebox.showerror("Error", "No file is opened to save.")
        return
    if file_path is None:
        file_path = ARM9_BIN_PATH
    try:
        with open(file_path, 'wb') as file:
            file.write(bytes(ARM_VALUES))
        messagebox.showinfo("Success", "Modified file saved successfully")
    except Exception as e:
        messagebox.showerror("Error", f"Failed to save modified file: {str(e)}")


def refresh_music_listbox():
    listboxmusic.delete(0, tk.END)
    for i, (music, offset) in enumerate(MUSIC_OFFSETS.items()):
        listboxmusic.insert(tk.END, f"{i}) {music} [{ARM_VALUES[offset]}]")


def refresh_file_listbox():
    listboxfile.delete(0, tk.END)
    for track, offsets in COURSE_OFFSETS.items():
        name_offset, size_offset = offsets
        track_name_bytes = bytes(ARM_VALUES[name_offset:size_offset])
        track_name = track_name_bytes.decode('utf-8', errors='replace').rstrip('\x00')
        listboxfile.insert(tk.END, f"{track} [{track_name}]")


def refresh_slot_listbox():
    listboxslot.delete(0, tk.END)
    for i, (slot, offset) in enumerate(SLOT_OFFSETS.items()):
        listboxslot.insert(tk.END, f"{i}) {slot} [{ARM_VALUES[offset]}]")


def refresh_emblem_listbox():
    listboxemblem.delete(0, tk.END)
    for character, offsets in EMBLEM_OFFSETS.items():
        name_offset, size_offset = offsets
        emblem_name_bytes = bytes(ARM_VALUES[name_offset:size_offset])
        emblem_name = emblem_name_bytes.decode('utf-8', errors='replace').rstrip('\x00')
        listboxemblem.insert(tk.END, f"{character} [{emblem_name}]")


def open_music_popup():
    root.attributes("-disabled", True)
    popup_window = tk.Toplevel(root)
    popup_window.title("Change SEQ Value")
    popup_window.geometry("300x180")
    popup_window.resizable(False, False)
    popup_window.attributes("-topmost", True)
    popup_window.iconphoto(True, get_icon_from_base64(ICON_BASE64))
    label = tk.Label(popup_window, text="Enter new SEQ value:")
    label.pack()
    entry = tk.Entry(popup_window)
    entry.pack()

    def change_seq_value():
        nonlocal popup_window
        selected_item = listboxmusic.get(listboxmusic.curselection())
        selected_index = int(selected_item.split(")")[0])
        track_name = list(MUSIC_OFFSETS.keys())[selected_index]
        offset = MUSIC_OFFSETS[track_name]
        try:
            new_value = int(entry.get())
            if 0 <= new_value <= 75:
                ARM_VALUES[offset] = new_value
                refresh_music_listbox()
                popup_window.destroy()
                root.attributes("-disabled", False)
                messagebox.showinfo("Success", f"SEQ value for {track_name} changed to {new_value}")
            else:
                popup_window.destroy()
                root.attributes("-disabled", False)
                messagebox.showerror("Error", "Invalid SEQ value. Value must be between 0 and 75.")
        except ValueError:
            popup_window.destroy()
            root.attributes("-disabled", False)
            messagebox.showerror("Error", "Invalid SEQ value. Value must be between 0 and 75.")

    button = tk.Button(popup_window, text="Change SEQ Value", command=change_seq_value)
    button.pack()
    cancel_button = tk.Button(popup_window, text="Cancel",
                              command=lambda: (popup_window.destroy(), root.attributes("-disabled", False)))
    cancel_button.pack()

    def close_popup_window():
        popup_window.destroy()
        root.attributes("-disabled", False)

    popup_window.protocol("WM_DELETE_WINDOW", close_popup_window)
    popup_window.transient(root)
    popup_window.grab_set()
    popup_window.mainloop()


def open_file_popup():
    root.attributes("-disabled", True)
    popup_window = tk.Toplevel(root)
    popup_window.title("Change File Name")
    popup_window.geometry("300x180")
    popup_window.resizable(False, False)
    popup_window.iconphoto(True, get_icon_from_base64(ICON_BASE64))
    label = tk.Label(popup_window, text="Enter new file name:")
    label.pack()
    entry = tk.Entry(popup_window)
    entry.pack()

    def change_file_name():
        nonlocal popup_window
        selected_index = listboxfile.curselection()
        if not selected_index:
            messagebox.showerror("ERROR", "No course selected.")
            return
        selected_index = int(selected_index[0])
        course_name = list(COURSE_OFFSETS.keys())[selected_index]
        name_offset, size_offset = COURSE_OFFSETS[course_name]
        new_name = entry.get()
        if not new_name:
            if not messagebox.askyesno("WARNING", "You have made the new name blank. Do you want to continue?"):
                return
            new_name = b'\x00' * (size_offset - name_offset)
        elif not re.match(r'^[A-Za-z0-9_]+$', new_name):
            messagebox.showerror("ERROR",
                                 "Invalid characters detected. Please use only English letters, numbers, and underscores.")
            return
        elif len(new_name) > (size_offset - name_offset):
            messagebox.showerror("ERROR", f"New file name exceeds block size ({size_offset - name_offset} bytes).")
            return
        else:
            new_name = new_name.encode('utf-8') + b'\x00' * (size_offset - name_offset - len(new_name))
        ARM_VALUES[name_offset:size_offset] = new_name
        refresh_file_listbox()
        popup_window.destroy()
        root.attributes("-disabled", False)
        messagebox.showinfo("Success", f"File name for {course_name} changed.")

    button = tk.Button(popup_window, text="Change Course Name", command=change_file_name)
    button.pack()
    cancel_button = tk.Button(popup_window, text="Cancel",
                              command=lambda: (popup_window.destroy(), root.attributes("-disabled", False)))
    cancel_button.pack()

    def close_popup_window():
        popup_window.destroy()
        root.attributes("-disabled", False)

    popup_window.protocol("WM_DELETE_WINDOW", close_popup_window)
    popup_window.transient(root)
    popup_window.grab_set()
    popup_window.mainloop()


def open_slot_popup():
    root.attributes("-disabled", True)
    popup_window = tk.Toplevel(root)
    popup_window.title("Change course Value")
    popup_window.geometry("300x920")
    popup_window.resizable(False, False)
    popup_window.attributes("-topmost", True)
    popup_window.iconphoto(True, get_icon_from_base64(ICON_BASE64))
    label = tk.Label(popup_window, text="Enter new course value:")
    label.pack()
    entry = tk.Entry(popup_window)
    entry.pack()

    def change_slot_value():
        nonlocal popup_window
        selected_item = listboxslot.get(listboxslot.curselection())
        selected_index = int(selected_item.split(")")[0])
        slot_name = list(SLOT_OFFSETS.keys())[selected_index]
        offset = SLOT_OFFSETS[slot_name]
        try:
            new_value = int(entry.get())
            if 0 < new_value < 55:
                ARM_VALUES[offset] = new_value
                refresh_slot_listbox()
                popup_window.destroy()
                root.attributes("-disabled", False)
                messagebox.showinfo("Success", f"course value for the {slot_name} changed to {new_value}")
            else:
                popup_window.destroy()
                root.attributes("-disabled", False)
                messagebox.showerror("Error", "Invalid course value. Value must be between 1 and 54.")
        except ValueError:
            popup_window.destroy()
            root.attributes("-disabled", False)
            messagebox.showerror("Error", "Invalid course value. Value must be between 1 and 54.")

    button = tk.Button(popup_window, text="Change course Value", command=change_slot_value)
    button.pack()
    cancel_button = tk.Button(popup_window, text="Cancel",
                              command=lambda: (popup_window.destroy(), root.attributes("-disabled", False)))
    cancel_button.pack()
    course_help = tk.Label(popup_window,
                           text="1 - GCN Yoshi Circuit\n2 - old_mario_gc\n3 - luigi_course\n4 - dokan_course\n5 - test1_course\n6 - donkey_course\n7 - wario_course\n8 - nokonoko_course\n9 - GCN Baby Park\n10 - SNES Mario Circuit 1\n11 - N64 Moo Mario Farm\n12 - GBA Bowser Castle 2\n13 - GBA Peach Circuit\n14 - GCN Luigi Circuit\n15 - SNES Koopa Beach 2\n16 - N64 Frappe Snowland (snow default)\n17 - Tick-Tock Clock\n18 - Luigi's Mansion\n19 - Airship Fortress\n20 - Figure-8 Circuit\n21 - test_circle\n22 - Yoshi Falls\n23 - N64 Banshee Boardwalk\n24 - Shroom Ridge\n25 - Mario Circuit\n26 - Peach Gardens\n27 - Desert Hills\n28 - Delfino Square\n29 - Rainbow Road\n30 - DK Pass\n31 - Cheep Cheep Beach\n32 - Bowser Castle\n33 - Waluigi Pinball\n34 - Wario Stadium (flashing lights default)\n35 - SNES Donut Plains 1\n36 - N64 Choco Mountain\n37 - GBA Luigi Circuit (rain default)\n38 - GCN Mushroom Bridge\n39 - SNES Choco Island 2\n40 - GBA Sky Garden\n41 - mini_block_course\n42 - Block Fort\n43 - Pipe Plaza\n44 - Nintendo DS\n45 - Twilight House\n46 - Palm Shore\n47 - Tart Top\n48 - mr_stage1\n49 - mr_stage2\n50 - mr_stage3\n51 - mr_stage4\n52 - Award\n53 - StaffRoll\n54 - StaffRollTrue")
    course_help.pack()

    def close_popup_window():
        popup_window.destroy()
        root.attributes("-disabled", False)

    popup_window.protocol("WM_DELETE_WINDOW", close_popup_window)
    popup_window.transient(root)
    popup_window.grab_set()
    popup_window.mainloop()


def open_emblem_popup():
    root.attributes("-disabled", True)
    popup_window = tk.Toplevel(root)
    popup_window.title("Change course Value")
    popup_window.geometry("300x920")
    popup_window.resizable(False, False)
    popup_window.attributes("-topmost", True)
    popup_window.iconphoto(True, get_icon_from_base64(ICON_BASE64))
    label = tk.Label(popup_window, text="Enter new course value:")
    label.pack()
    entry = tk.Entry(popup_window)
    entry.pack()

    def change_emblem_value():
        nonlocal popup_window
        selected_item = listboxemblem.get(listboxemblem.curselection())
        selected_index = int(selected_item.split(")")[0])
        emblem_name = list(EMBLEM_OFFSETS.keys())[selected_index]
        offset = EMBLEM_OFFSETS[emblem_name]
        try:
            new_value = int(entry.get())
            if 0 < new_value < 55:
                EMBLEM_OFFSETS[offset] = new_value
                refresh_emblem_listbox()
                popup_window.destroy()
                root.attributes("-disabled", False)
                messagebox.showinfo("Success", f"course value for the {emblem_name} changed to {new_value}")
            else:
                popup_window.destroy()
                root.attributes("-disabled", False)
                messagebox.showerror("Error", "Invalid course value. Value must be between 1 and 54.")
        except ValueError:
            popup_window.destroy()
            root.attributes("-disabled", False)
            messagebox.showerror("Error", "Invalid course value. Value must be between 1 and 54.")

    button = tk.Button(popup_window, text="Change course Value", command=change_emblem_value)
    button.pack()
    cancel_button = tk.Button(popup_window, text="Cancel",
                              command=lambda: (popup_window.destroy(), root.attributes("-disabled", False)))
    cancel_button.pack()

    def close_popup_window():
        popup_window.destroy()
        root.attributes("-disabled", False)

    popup_window.protocol("WM_DELETE_WINDOW", close_popup_window)
    popup_window.transient(root)
    popup_window.grab_set()
    popup_window.mainloop()


def save_file_as():
    file_path = filedialog.asksaveasfilename(defaultextension=".bin",
                                             filetypes=(("Binary Files", "*.bin"), ("All Files", "*.*")))
    if file_path:
        save_file(file_path)


def open_help():
    messagebox.showinfo("Help",
                        "This program allows you to edit many values in the arm9.bin file of Mario Kart DS.\n\n"
                        "Code: Landon & Emma\n"
                        "Special Thanks: Ermelber, Yami, MkDasher")


def open_repository():
    webbrowser.open_new("https://github.com/LandonAndEmma/MKDS-ARM9-Editor")


def on_closing():
    if not ARM9_BIN_PATH or messagebox.askokcancel("Quit", "Do you want to quit?"):
        root.destroy()


root = tk.Tk()
root.title("Mario Kart DS ARM9 Editor")
root.geometry("600x400")
root.iconphoto(True, get_icon_from_base64(ICON_BASE64))
menubar = tk.Menu(root)
file_menu = tk.Menu(menubar, tearoff=0)
file_menu.add_command(label="Open", command=open_file)
file_menu.add_command(label="Save", command=save_file)
file_menu.add_command(label="Save As", command=save_file_as)
menubar.add_cascade(label="File", menu=file_menu)
help_menu = tk.Menu(menubar, tearoff=0)
help_menu.add_command(label="Help", command=open_help)
help_menu.add_command(label="Repository", command=open_repository)
menubar.add_cascade(label="Help", menu=help_menu)
root.config(menu=menubar)
main_frame = tk.Frame(root)
main_frame.pack(fill=tk.BOTH, expand=True)
tabControl = ttk.Notebook(main_frame)
tab1 = ttk.Frame(tabControl)
tab2 = ttk.Frame(tabControl)
tab3 = ttk.Frame(tabControl)
tab4 = ttk.Frame(tabControl)
tabControl.add(tab1, text='Music IDs')
tabControl.add(tab2, text='Course Filenames')
tabControl.add(tab3, text='Weather Slots')
tabControl.add(tab4, text='Emblem Filenames')
tabControl.pack(expand=1, fill="both")
listboxmusic = tk.Listbox(tab1)
listboxmusic.pack(fill=tk.BOTH, expand=True)
listboxfile = tk.Listbox(tab2)
listboxfile.pack(fill=tk.BOTH, expand=True)
listboxslot = tk.Listbox(tab3)
listboxslot.pack(fill=tk.BOTH, expand=True)
listboxemblem = tk.Listbox(tab4)
listboxemblem.pack(fill=tk.BOTH, expand=True)


def on_listbox_music_select(event):
    selected_index = listboxmusic.curselection()
    if selected_index:
        open_music_popup()


def on_listbox_file_select(event):
    selected_index = listboxfile.curselection()
    if selected_index:
        open_file_popup()


def on_listbox_slot_select(event):
    selected_index = listboxslot.curselection()
    if selected_index:
        open_slot_popup()


def on_listbox_emblem_select(event):
    selected_index = listboxemblem.curselection()
    if selected_index:
        open_emblem_popup()


listboxmusic.bind("<<ListboxSelect>>", on_listbox_music_select)
listboxfile.bind("<<ListboxSelect>>", on_listbox_file_select)
listboxslot.bind("<<ListboxSelect>>", on_listbox_slot_select)
listboxemblem.bind("<<ListboxSelect>>", on_listbox_emblem_select)
root.protocol("WM_DELETE_WINDOW", on_closing)
root.mainloop()
